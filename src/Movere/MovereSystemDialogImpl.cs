using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Platform;
using AvaloniaFilter = Avalonia.Controls.FileDialogFilter;

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
                var service = new OpenFileDialogService(host);

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

                var result = await service.ShowDialogAsync(options);
                return result.SelectedPaths.ToArray();
            }

            if (dialog is SaveFileDialog saveFileDialog)
            {
                var service = new SaveFileDialogService(host);

                var options = new SaveFileDialogOptions()
                {
                    //Filters = dialog.Filters.Select(ConvertFilter).ToImmutableArray(),
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

                var result = await service.ShowDialogAsync(options);
                return result.SelectedPath is null ? [] : [result.SelectedPath];
            }

            throw new NotImplementedException();
        }

        public Task<string?> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent) =>
            throw new NotImplementedException();

        private static MovereFilter ConvertFilter(AvaloniaFilter filter) =>
            new MovereFilter(filter.Name ?? String.Join(", ", filter.Extensions), filter.Extensions.ToImmutableArray());
    }
}
