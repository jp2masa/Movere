using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;
using Movere.Sample.Views;
using Movere.Services;

namespace Movere.Sample
{
    internal class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();

                var messageDialogService = new MessageDialogService(mainWindow);

                var openFileDialogService = new OpenFileDialogService(mainWindow);
                var saveFileDialogService = new SaveFileDialogService(mainWindow);

                var printDialogService = new PrintDialogService(mainWindow);

                mainWindow.DataContext = new MainWindowViewModel(
                    messageDialogService,
                    openFileDialogService,
                    saveFileDialogService,
                    printDialogService);

                desktop.MainWindow = mainWindow;
            }
        }

        [STAThread]
        private static int Main(string[] args) =>
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);

        private static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .LogToDebug()
#endif
                .UseReactiveUI();
    }
}
