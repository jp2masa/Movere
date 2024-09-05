using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using NuGet.Configuration;
using NuGet.Versioning;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

using Octokit;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    private const string MyGetFeedUrl =
        "https://www.myget.org/F/jp2masa/api/v2/package";

    public static int Main() =>
        Execute<Build>(x => x.Pack);

    [Parameter, Secret]
    private readonly string? MyGetApiKey;

    [Parameter, Secret]
    private readonly string? NuGetApiKey;

    [Parameter, Secret]
    private readonly string? GitHubToken;

    [Parameter(
        "Configuration to build - Default is 'Debug' (local) or 'Release'" +
        " (server)"
    )]
    private readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Parameter]
    private readonly long BuildNumberOffset;

    [GitRepository]
    private readonly GitRepository Repository;

    private GitHubActions GitHubActions =>
        GitHubActions.Instance;

    private AbsolutePath MovereSlnPath =>
        RootDirectory / "Movere.sln";

    private AbsolutePath ArtifactsPath =>
        RootDirectory / "artifacts" / Configuration;

    private AbsolutePath NupkgArtifactsPath =>
        ArtifactsPath / "nupkg";

    private string? BranchName =>
        StringEqualsOrdinalIgnoreCase(GitHubActions.RefType, "branch")
            ? GitHubActions.RefName
            : null;

    private string? TagName =>
        StringEqualsOrdinalIgnoreCase(GitHubActions.RefType, "tag")
            ? GitHubActions.RefName
            : null;

    private SemanticVersion? TagVersion =>
        TagName is not null
        && TagName.Length >= 2
        && TagName[0] == 'v'
        && SemanticVersion.TryParse(TagName[1..], out var version)
            ? version
            : null;

    private long BuildNumber =>
        BuildNumberOffset + GitHubActions.RunNumber;

    private string PackageVersionSuffix =>
        IsLocalBuild
            ? "-local"
            : (
                TagName is not null
                    ? ""
                    : $"-build2.{BuildNumber}+{GitHubActions.Sha[0..7]}"
            );


    private Target Restore => _ => _
        .Executes(
            () => DotNetTasks.DotNetRestore(
                x => x
                    .SetProjectFile(MovereSlnPath)
                    .AddProperty("PackageVersionSuffix", PackageVersionSuffix)
            )
        );

    private Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(
            () => DotNetTasks.DotNetBuild(
                x => x
                    .SetProjectFile(MovereSlnPath)
                    .AddProperty("PackageVersionSuffix", PackageVersionSuffix)
            )
        );

    private Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(
            () => DotNetTasks.DotNetPack(
                x => x
                    .SetProject(MovereSlnPath)
                    .SetConfiguration(Configuration)
                    .AddProperty("PackageVersionSuffix", PackageVersionSuffix)
            )
        );

    private Target PushMyGet => _ => _
        .TriggeredBy(Pack)
        .OnlyWhenStatic(
            () =>
                !GitHubActions.IsPullRequest
                && Configuration == Configuration.Debug
        )
        .Requires(() => MyGetApiKey)
        .Executes(
            () =>
                NupkgArtifactsPath
                    .GetFiles()
                    .ForEach(
                        nupkg => DotNetTasks.DotNetNuGetPush(
                            x => x
                                .SetSource(MyGetFeedUrl)
                                .SetApiKey(MyGetApiKey)
                                .SetTargetPath(nupkg)
                        )
                    )
        );

    private Target PushNuGet => _ => _
        .TriggeredBy(Pack)
        .OnlyWhenStatic(
            () =>
                TagVersion is not null
                && Configuration == Configuration.Release
        )
        .Requires(() => NuGetApiKey)
        .Executes(
            () =>
                NupkgArtifactsPath
                    .GetFiles()
                    .ForEach(
                        nupkg => DotNetTasks.DotNetNuGetPush(
                            x => x
                                .SetSource(NuGetConstants.V3FeedUrl)
                                .SetApiKey(NuGetApiKey)
                                .SetTargetPath(nupkg)
                        )
                    )
        );

    private Target ReleaseGitHub => _ => _
        .TriggeredBy(Pack, PushNuGet)
        .OnlyWhenStatic(
            () =>
                TagVersion is not null
                && Configuration == Configuration.Release
        )
        .Requires(() => GitHubToken)
        .Executes(
            async () =>
            {
                var repositoryId = GetVariable<long>("GITHUB_REPOSITORY_ID");

                var client = new GitHubClient(
                    new ProductHeaderValue("jp2masa"),
                    new Octokit.Internal.InMemoryCredentialStore(
                        new Credentials(GitHubToken)
                    )
                );

                var release = await client
                    .Repository
                    .Release
                    .Create(
                        repositoryId,
                        new NewRelease(TagName!)
                        {
                            Draft = true,
                            Name = TagName!,
                            Prerelease = TagVersion!.IsPrerelease
                        }
                );

                // preserve order
                foreach (var nupkg in NupkgArtifactsPath.GetFiles())
                {
                    using var ms = new MemoryStream(nupkg.ReadAllBytes());

                    await client
                        .Repository
                        .Release
                        .UploadAsset(
                            release,
                            new ReleaseAssetUpload(
                                nupkg.Name,
                                "application/octet-stream",
                                ms,
                                null
                            )
                        );
                }

                await client.Repository.Release.Edit(
                    repositoryId,
                    release.Id,
                    new ReleaseUpdate() { Draft = false }
                );
            }
        );

    private static bool StringEqualsOrdinalIgnoreCase(string? x, string? y) =>
        String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
}
