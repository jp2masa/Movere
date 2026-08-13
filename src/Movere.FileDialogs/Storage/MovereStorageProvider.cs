using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

using Movere.Models;
using Movere.Services;
using MovereFilter = Movere.Models.FileDialogFilter;

namespace Movere.Storage
{
    internal sealed class MovereStorageProvider(
        Func<IDialogHost> hostFactory,
        MovereStorageProviderOptions options
    )
        : BclStorageProvider
    {
        private static readonly OpenFilePickerResult s_cancelOpenFilePickerResult =
            new OpenFilePickerResult();

        private static readonly SaveFilePickerResult s_cancelSaveFilePickerResult =
            new SaveFilePickerResult();

        public override bool CanOpen => true;

        public override bool CanSave => true;

        public override bool CanPickFolder => false;

        public override async Task<OpenFilePickerResult> OpenFilePickerWithResultAsync(FilePickerOpenOptions options)
        {
            await using var host = hostFactory();

            var convertedOptions = new OpenFileDialogOptions()
            {
                AllowMultipleSelection = options.AllowMultiple,
                Filters = options.FileTypeFilter?.Select(ConvertFilter).ToImmutableArray()
                    ?? ImmutableArray<MovereFilter>.Empty,
                InitialDirectory = TryConvertStorageFolder(options.SuggestedStartLocation, checkIfExists: true),
                InitialFileName = options.SuggestedFileName
            };

            if (options.Title is { } title)
            {
                // no conditional assignment of init properties
                // (https://github.com/dotnet/csharplang/discussions/5588)
                convertedOptions = convertedOptions with { Title = title };
            }

            var result = await host.ShowOpenFileDialogAsync(convertedOptions);

            return result
                .Match(
                    open =>
                        new OpenFilePickerResult()
                        {
                            Files = open
                                .SelectedPaths
                                .Select(static x => new BclStorageFile(new FileInfo(x)))
                                .ToImmutableArray(),
                            SelectedFileType = open.SelectedFilter is { } filter
                                ? options.FileTypeFilter
                                    ?.First(x => String.Equals(x.Name, filter.Name, StringComparison.Ordinal))
                                : null
                        },
                    cancel => s_cancelOpenFilePickerResult
                );
        }

        public override async Task<SaveFilePickerResult> SaveFilePickerWithResultAsync(FilePickerSaveOptions options)
        {
            await using var host = hostFactory();

            var convertedOptions = new SaveFileDialogOptions()
            {
                DefaultExtension = options.DefaultExtension is null
                    ? null
                    : RemovePrefix(options.DefaultExtension, '.'),
                Filters = options.FileTypeChoices?.Select(ConvertFilter).ToImmutableArray()
                          ?? ImmutableArray<MovereFilter>.Empty,
                InitialDirectory = TryConvertStorageFolder(options.SuggestedStartLocation, checkIfExists: true),
                InitialFileName = options.SuggestedFileName,
                ShowOverwritePrompt = options.ShowOverwritePrompt ?? true
            };

            if (options.Title is { } title)
            {
                // no conditional assignment of init properties
                // (https://github.com/dotnet/csharplang/discussions/5588)
                convertedOptions = convertedOptions with { Title = title };
            }

            var result = await host.ShowSaveFileDialogAsync(convertedOptions);

            return result
                .Match(
                    save =>
                        new SaveFilePickerResult()
                        {
                            File = new BclStorageFile(new FileInfo(save.SelectedPath)),
                            SelectedFileType = save.SelectedFilter is { } filter
                                ? options.FileTypeChoices
                                    ?.First(x => String.Equals(x.Name, filter.Name, StringComparison.Ordinal))
                                : null
                        },
                    cancel => s_cancelSaveFilePickerResult
                );
        }

        public override Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options) =>
            throw new NotSupportedException();

        private static MovereFilter ConvertFilter(FilePickerFileType filter) =>
            new MovereFilter(filter.Name, GetExtensions(filter));

        private static DirectoryInfo? TryConvertStorageFolder(IStorageFolder? folder, bool checkIfExists = false) =>
            folder is IStorageItemWithFileSystemInfo item
            && item.FileSystemInfo is DirectoryInfo directory
            && (!checkIfExists || directory.Exists)
            ? directory
            : null;

        private static ImmutableArray<string> GetExtensions(FilePickerFileType filter) =>
            (
                filter.Patterns?.Select(x => RemovePrefix(x, "*."))
            //?? filter.MimeTypes
            //?? filter.AppleUniformTypeIdentifiers
            )
                ?.ToImmutableArray()
                ?? ImmutableArray<string>.Empty;

        internal static string RemovePrefix(string str, string prefix) =>
            str.StartsWith(prefix, StringComparison.Ordinal)
                ? str.Substring(prefix.Length)
                : str;

        internal static string RemovePrefix(string str, char prefix) =>
            str.StartsWith(prefix)
                ? str.Substring(1)
                : str;
    }
}
