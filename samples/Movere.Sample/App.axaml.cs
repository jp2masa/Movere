using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;
using Movere.Sample.Views;
using Movere.Services;
using Movere.Win32;

namespace Movere.Sample
{
    internal partial class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();

                var messageDialogService = new MessageDialogService(mainWindow);

                var contentDialogService = new ContentDialogService<CustomContentViewModel, FormResult>(
                    mainWindow,
                    new CustomContentViewResolver());

                var openFileDialogService = new OpenFileDialogService(mainWindow);
                var saveFileDialogService = new SaveFileDialogService(mainWindow);

                var printDialogService = new PrintDialogService(mainWindow);

                mainWindow.DataContext = new MainWindowViewModel(
                    () => AvaloniaOpenFile(mainWindow),
                    () => AvaloniaSaveFile(mainWindow),
                    messageDialogService,
                    contentDialogService,
                    openFileDialogService,
                    saveFileDialogService,
                    printDialogService);

                desktop.MainWindow = mainWindow;
            }
        }

        [STAThread]
        private static int Main(string[] args) =>
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);

        private static AppBuilder BuildAvaloniaApp() =>
            AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .LogToTrace()
#endif
                .UseMovere()
                .UseMovereWin32()
                .UseReactiveUI();

        private static Task AvaloniaOpenFile(Window parent)
        {
            var dialog = new OpenFileDialog()
            {
                AllowMultiple = true,
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "Picture files", Extensions = new List<string>() { "png", "jpg" } },
                    new FileDialogFilter() { Name = "Music files", Extensions = new List<string>() { "mp3", "wav" } }
                }
            };

            return dialog.ShowAsync(parent);
        }

        private static Task AvaloniaSaveFile(Window parent)
        {
            var dialog = new SaveFileDialog()
            {
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "Picture files", Extensions = new List<string>() { "png", "jpg" } }
                }
            };

            return dialog.ShowAsync(parent);
        }
    }
}
