using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Services;
using Movere.ViewModels;

namespace Movere.Sample.ViewModels
{
    internal enum FormResult
    {
        OK,
        Wait,
        Cancel
    }

    internal class MainWindowViewModel : ReactiveObject
    {
        private readonly MessageDialogService _messageDialogService;
        private readonly ContentDialogService<CustomContentViewModel, FormResult> _contentDialogService;

        private readonly OpenFileDialogService _openFileDialogService;
        private readonly SaveFileDialogService _saveFileDialogService;

        private readonly PrintDialogService _printDialogService;

        private string _messageDialogResult = "Not opened yet";
        private string _contentDialogResult = "Not opened yet";

        public MainWindowViewModel(
            Func<Task> avaloniaOpenFile,
            Func<Task> avaloniaSaveFile,
            MessageDialogService messageDialogService,
            ContentDialogService<CustomContentViewModel, FormResult> contentDialogService,
            OpenFileDialogService openFileDialogService,
            SaveFileDialogService saveFileDialogService,
            PrintDialogService printDialogService)
        {
            _messageDialogService = messageDialogService;
            _contentDialogService = contentDialogService;

            _openFileDialogService = openFileDialogService;
            _saveFileDialogService = saveFileDialogService;

            _printDialogService = printDialogService;

            ShowMessageCommand = ReactiveCommand.Create(ShowMessageAsync);
            ShowCustomContentCommand = ReactiveCommand.Create(ShowCustomContentAsync);

            OpenFileCommand = ReactiveCommand.Create(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.Create(SaveFileAsync);

            PrintCommand = ReactiveCommand.Create(PrintAsync);

            AvaloniaOpenFileCommand = ReactiveCommand.Create(avaloniaOpenFile);
            AvaloniaSaveFileCommand = ReactiveCommand.Create(avaloniaSaveFile);
        }

        public string MessageDialogResult
        {
            get => _messageDialogResult;
            set => this.RaiseAndSetIfChanged(ref _messageDialogResult, value);
        }

        public string ContentDialogResult
        {
            get => _contentDialogResult;
            set => this.RaiseAndSetIfChanged(ref _contentDialogResult, value);
        }

        public ICommand ShowMessageCommand { get; }

        public ICommand ShowCustomContentCommand { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand SaveFileCommand { get; }

        public ICommand PrintCommand { get; }

        public ICommand AvaloniaOpenFileCommand { get; }

        public ICommand AvaloniaSaveFileCommand { get; }

        private async Task ShowMessageAsync() =>
            MessageDialogResult = (
                await _messageDialogService.ShowMessageDialogAsync(
                    new MessageDialogOptions(
                        "Some really really really really really really really really really really really " +
                        "really really really really really really really really really really really really " +
                        "really really really really really really really really really really really really " +
                        "really really really really really really really really really long message",
                        "Message Dialog",
                        DialogIcon.Error,
                        DialogResultSet.AbortRetryIgnore
                    )
                )
            )
                .Name.GetString() ?? "null";

        private async Task ShowCustomContentAsync()
        {
            var id = new FieldViewModel("ID");
            var firstName = new FieldViewModel("First Name");
            var lastName = new FieldViewModel("Last Name");

            var fields = new FieldViewModel[] { id, firstName, lastName };

            var vm = new CustomContentViewModel(fields);

            var actions = ImmutableArray.Create(
                DialogAction.Create(
                    DialogResult.OK.Name,
                    ReactiveCommand.Create(
                        (CustomContentViewModel x) => FormResult.OK,
                        Observable.CombineLatest(
                            from field in fields
                            select from value in field.WhenAnyValue(x => x.Value)
                                    select !String.IsNullOrWhiteSpace(value),
                            x => x.All(y => y)
                        )
                    )
                ),
                DialogAction.Create(
                    "Wait",
                    ReactiveCommand.CreateFromTask(
                        async (CustomContentViewModel x) =>
                        {
                            await Task.Delay(5000);
                            return FormResult.Wait;
                        }
                    )
                ),
                DialogAction.Create(
                    DialogResult.Cancel.Name,
                    ReactiveCommand.Create((CustomContentViewModel x) => FormResult.Cancel)
                )
            );

            var result = await _contentDialogService.ShowDialogAsync(
                ContentDialogOptions.Create("Custom content", vm, DialogActionSet.Create(actions, actions[2], actions[0]))
            );

            ContentDialogResult = $"Result: {result}    ID: {id.Value}    First Name: {firstName.Value}    Last Name: {lastName.Value}";
        }

        private Task OpenFileAsync() => _openFileDialogService.ShowDialogAsync(true);

        private Task SaveFileAsync() => _saveFileDialogService.ShowDialogAsync();

        private async Task PrintAsync()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            using var document = new PrintDocument();

            document.PrintPage += PrintDocument;
            await _printDialogService.ShowDialogAsync(document);
        }

        private static void PrintDocument(object sender, PrintPageEventArgs e)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            using var font = new Font(FontFamily.GenericSansSerif, 100, FontStyle.Regular);
            e.Graphics?.DrawString("Hello World!", font, Brushes.Green, new PointF(4, 4));
        }
    }
}
