using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.Sample.ViewModels
{
    internal class MainWindowViewModel : ReactiveObject
    {
        private readonly MessageDialogService _messageDialogService;

        private readonly OpenFileDialogService _openFileDialogService;
        private readonly SaveFileDialogService _saveFileDialogService;

        private readonly PrintDialogService _printDialogService;

        public MainWindowViewModel(
            MessageDialogService messageDialogService,
            OpenFileDialogService openFileDialogService,
            SaveFileDialogService saveFileDialogService,
            PrintDialogService printDialogService)
        {
            _messageDialogService = messageDialogService;

            _openFileDialogService = openFileDialogService;
            _saveFileDialogService = saveFileDialogService;

            _printDialogService = printDialogService;

            ShowMessageCommand = ReactiveCommand.Create(ShowMessageAsync);

            OpenFileCommand = ReactiveCommand.Create(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.Create(SaveFileAsync);

            PrintCommand = ReactiveCommand.Create(PrintAsync);
        }

        public ICommand OpenFileCommand { get; }

        public ICommand SaveFileCommand { get; }

        public ICommand PrintCommand { get; }

        public ICommand ShowMessageCommand { get; }

        private Task ShowMessageAsync() =>
            _messageDialogService.ShowMessageDialogAsync(
                new MessageDialogOptions(
                    DialogIcon.Error,
                    "Message Dialog",
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    "Some really really really really really really really really really really really " +
                    "really really really really really really really really really really really really " +
                    "really really really really really really really really really really really really " +
                    "really really really really really really really really really long message",
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    DialogResultSet.AbortRetryIgnore));

        private Task OpenFileAsync() => _openFileDialogService.ShowDialogAsync();

        private Task SaveFileAsync() => _saveFileDialogService.ShowDialogAsync();

        private async Task PrintAsync()
        {
            using var document = new PrintDocument();

            document.PrintPage += PrintDocument;
            await _printDialogService.ShowDialogAsync(document);
        }

        private static void PrintDocument(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Hello World!", SystemFonts.DefaultFont, Brushes.Black, new PointF(4, 4));
        }
    }
}
