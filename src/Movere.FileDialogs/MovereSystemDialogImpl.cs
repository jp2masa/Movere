using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Platform;
#pragma warning disable CS0618 // Type or member is obsolete
using AvaloniaFilter = Avalonia.Controls.FileDialogFilter;
#pragma warning restore CS0618 // Type or member is obsolete

using Movere.Avalonia.Services;
using Movere.Models;
using Movere.Services;
using Movere.Storage;
using MovereFilter = Movere.Models.FileDialogFilter;

namespace Movere
{
    [Obsolete]
    internal sealed class MovereSystemDialogImpl : ISystemDialogImpl
    {
        public async Task<string[]?> ShowFileDialogAsync(FileDialog dialog, Window parent)
        {
            var application = MovereStorageProviderFactory.GetApplication();

            await using var host = new WindowDialogHost(application, parent);

            if (dialog is OpenFileDialog openFileDialog)
            {
                var options = new OpenFileDialogOptions()
                {
                    AllowMultipleSelection = openFileDialog.AllowMultiple,
                    Filters = dialog.Filters.Select(ConvertFilter).ToImmutableArray(),
                    InitialDirectory =
                        String.IsNullOrWhiteSpace(dialog.Directory)
                        || new DirectoryInfo(dialog.Directory) is not { Exists: true } initialDirectory
                        ? null
                        : initialDirectory,
                    InitialFileName = dialog.InitialFileName
                };

                if (dialog.Title is { } title)
                {
                    // no conditional assignment of init properties
                    // (https://github.com/dotnet/csharplang/discussions/5588)
                    options = options with { Title = title };
                }

                var result = await host.ShowOpenFileDialogAsync(options);

                return result
                    .Match<string[]?>(
                        open => open.SelectedPaths.ToArray(),
                        cancel => null
                    );
            }

            if (dialog is SaveFileDialog saveFileDialog)
            {
                var options = new SaveFileDialogOptions()
                {
                    DefaultExtension = saveFileDialog.DefaultExtension is null
                        ? null
                        : MovereStorageProvider.RemovePrefix(
                            saveFileDialog.DefaultExtension,
#if NETSTANDARD2_0
                            "."
#else
                            '.'
#endif
                        ),
                    Filters = dialog.Filters.Select(ConvertFilter).ToImmutableArray(),
                    InitialDirectory =
                        String.IsNullOrWhiteSpace(dialog.Directory)
                        || new DirectoryInfo(dialog.Directory) is not { Exists: true } initialDirectory
                            ? null
                            : initialDirectory,
                    InitialFileName = dialog.InitialFileName,
                    ShowOverwritePrompt = saveFileDialog.ShowOverwritePrompt ?? true
                };

                if (dialog.Title is { } title)
                {
                    // no conditional assignment of init properties
                    // (https://github.com/dotnet/csharplang/discussions/5588)
                    options = options with { Title = title };
                }

                var result = await host.ShowSaveFileDialogAsync(options);

                return result
                    .Match<string[]?>(
                        save => [save.SelectedPath],
                        cancel => null
                    );
            }

            throw new NotSupportedException();
        }

        public Task<string?> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent) =>
            throw new NotSupportedException();

        private static MovereFilter ConvertFilter(AvaloniaFilter filter) =>
            new MovereFilter(filter.Name ?? String.Join(", ", filter.Extensions), filter.Extensions.ToImmutableArray());
    }
}
