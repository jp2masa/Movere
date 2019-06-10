using System.Diagnostics;

using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Serilog;

using Movere.Sample.ViewModels;
using Movere.Sample.Views;
using Movere.Services;

namespace Movere.Sample
{
    internal class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void Main()
        {
            InitializeLogging();

            var builder = BuildAvaloniaApp();
            builder.SetupWithoutStarting();

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

            builder.Instance.Run(mainWindow);
        }

        private static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI();

        [Conditional("DEBUG")]
        private static void InitializeLogging() =>
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
    }
}
