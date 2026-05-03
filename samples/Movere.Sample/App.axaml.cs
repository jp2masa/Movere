using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

using ReactiveUI.Avalonia;

using Movere.Avalonia.Services;
using Movere.Avalonia.Storage;
using Movere.FileDialogs;
using Movere.Models;
using Movere.PrintDialog;
using Movere.Sample.ViewModels;
using Movere.Sample.Views;
using Movere.Win32;

namespace Movere.Sample
{
    internal sealed class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();

                var customContentViewResolver = new CustomContentViewResolver();

                var windowHost = new WindowDialogHost(this, mainWindow, customContentViewResolver);
                var overlayHost = new OverlayDialogHost(this, mainWindow, customContentViewResolver);

                mainWindow.DataContext = new MainWindowViewModel(
                    windowHost,
                    overlayHost,
                    () => AvaloniaOpenFile(mainWindow),
                    () => AvaloniaSaveFile(mainWindow)
                );

                desktop.MainWindow = mainWindow;
            }
        }

        [STAThread]
        private static int Main(string[] args) =>
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);

        private static AppBuilder BuildAvaloniaApp() =>
#pragma warning disable CS0612 // Type or member is obsolete
            AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .LogToTrace()
#endif
                .UseMovereFileDialogs()
                .UseMovereStorageProvider(new MovereStorageProviderOptions() { IsFallback = false })
                .UseMoverePrintDialogs()
#pragma warning restore CS0612 // Type or member is obsolete
                .UseMovereWin32()
                .UseReactiveUI(static _ => { });

        private static Task AvaloniaOpenFile(TopLevel parent)
        {
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var options = new FilePickerOpenOptions()
            {
                AllowMultiple = true,
                SuggestedStartLocation = new Folder(new DirectoryInfo(userProfilePath)).ToStorageFolder(),
                FileTypeFilter =
                [
                    new FilePickerFileType("Picture files") { Patterns = new List<string>() { "png", "jpg" } },
                    new FilePickerFileType("Music files") { Patterns = new List<string>() { "mp3", "wav" } }
                ]
            };

            return parent.StorageProvider.OpenFilePickerAsync(options);
        }

        private static Task AvaloniaSaveFile(TopLevel parent)
        {
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var options = new FilePickerSaveOptions()
            {
                SuggestedStartLocation = new Folder(new DirectoryInfo(userProfilePath)).ToStorageFolder(),
                FileTypeChoices =
                [
                    FilePickerFileTypes.ImageAll,
                    FilePickerFileTypes.Pdf
                ]
            };

            return parent.StorageProvider.SaveFilePickerAsync(options);
        }
    }
}
