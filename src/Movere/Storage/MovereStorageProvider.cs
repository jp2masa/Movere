﻿using System;
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
        public override bool CanOpen => true;

        public override bool CanSave => true;

        public override bool CanPickFolder => false;

        public override async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
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

            return result.SelectedPaths
                .Select(static x => new BclStorageFile(new FileInfo(x)))
                .ToImmutableArray();
        }

        public override async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            await using var host = hostFactory();

            // TODO: implement FileTypeChoices and DefaultExtension
            // TODO: implement ShowOverwritePrompt (it's always shown)
            var convertedOptions = new SaveFileDialogOptions()
            {
                //Filters = dialog.Filters.Select(ConvertFilter).ToImmutableArray(),
                InitialDirectory = TryConvertStorageFolder(options.SuggestedStartLocation, checkIfExists: true),
                InitialFileName = options.SuggestedFileName
            };

            if (options.Title is { } title)
            {
                // no conditional assignment of init properties
                // (https://github.com/dotnet/csharplang/discussions/5588)
                convertedOptions = convertedOptions with { Title = title };
            }

            var result = await host.ShowSaveFileDialogAsync(convertedOptions);

            return result.SelectedPath is null
                ? null
                : new BclStorageFile(new FileInfo(result.SelectedPath));
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
                filter.Patterns
                    ?? filter.MimeTypes
                    ?? filter.AppleUniformTypeIdentifiers
                    ?? Array.Empty<string>()
            )
                .ToImmutableArray();
    }
}
