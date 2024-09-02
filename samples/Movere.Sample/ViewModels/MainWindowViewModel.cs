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
        private sealed class DialogServices(IDialogHost host)
        {
            public IMessageDialogService Message { get; } =
                new MessageDialogService(host);

            public IContentDialogService<CustomContentViewModel, FormResult> Content { get; } =
                new ContentDialogService<CustomContentViewModel, FormResult>(host);

            public IOpenFileDialogService OpenFile { get; } =
                new OpenFileDialogService(host);

            public ISaveFileDialogService SaveFile { get; } =
                new SaveFileDialogService(host);

            public IPrintDialogService Print { get; } =
                new PrintDialogService(host);
        }

        private readonly DialogServices _windowDialogServices;
        private readonly DialogServices _overlayDialogServices;

        private string _messageDialogResult = "Not opened yet";
        private string _contentDialogResult = "Not opened yet";

        private bool _useOverlayDialogs = false;

        public MainWindowViewModel(
            IDialogHost windowHost,
            IDialogHost overlayHost,
            Func<Task> avaloniaOpenFile,
            Func<Task> avaloniaSaveFile,
            Func<Task> avaloniaOldOpenFile,
            Func<Task> avaloniaOldSaveFile
        )
        {
            _windowDialogServices = new DialogServices(windowHost);
            _overlayDialogServices = new DialogServices(overlayHost);

            ShowMessageCommand = ReactiveCommand.CreateFromTask(ShowMessageAsync);
            ShowCustomContentCommand = ReactiveCommand.CreateFromTask(ShowCustomContentAsync);

            OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.CreateFromTask(SaveFileAsync);

            PrintCommand = ReactiveCommand.CreateFromTask(PrintAsync);

            AvaloniaOpenFileCommand = ReactiveCommand.CreateFromTask(avaloniaOpenFile);
            AvaloniaSaveFileCommand = ReactiveCommand.CreateFromTask(avaloniaSaveFile);

            AvaloniaOldOpenFileCommand = ReactiveCommand.CreateFromTask(avaloniaOldOpenFile);
            AvaloniaOldSaveFileCommand = ReactiveCommand.CreateFromTask(avaloniaOldSaveFile);
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

        public ICommand AvaloniaOldOpenFileCommand { get; }

        public ICommand AvaloniaOldSaveFileCommand { get; }

        public bool UseOverlayDialogs
        {
            get => _useOverlayDialogs;
            set => this.RaiseAndSetIfChanged(ref _useOverlayDialogs, value);
        }

        private DialogServices Dialogs =>
            UseOverlayDialogs
                ? _overlayDialogServices
                : _windowDialogServices;

        private async Task ShowMessageAsync() =>
            MessageDialogResult = (
                await Dialogs.Message.ShowMessageDialogAsync(
                    new MessageDialogOptions(
                        "Some really really really really really really really really really really really " +
                        "really really really really really really really really really really really really " +
                        "really really really really really really really really really really really really " +
                        "really really really really really really really really really long message",
                        (LocalizedString)"Message Dialog"
                    )
                    {
                        Icon = AvaloniaDialogIcon.Error,
                        DialogResults = DialogResultSet.AbortRetryIgnore
                    }
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

            var result = await Dialogs.Content.ShowDialogAsync(
                ContentDialogOptions.Create("Custom content", vm, DialogActionSet.Create(actions, actions[2], actions[0]))
            );

            ContentDialogResult = $"Result: {result}    ID: {id.Value}    First Name: {firstName.Value}    Last Name: {lastName.Value}";
        }

        private Task OpenFileAsync() =>
            Dialogs.OpenFile.ShowDialogAsync(
                new OpenFileDialogOptions()
                {
                    AllowMultipleSelection = true
                }
            );

        private Task SaveFileAsync() =>
            Dialogs.SaveFile.ShowDialogAsync();

        private async Task PrintAsync()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            using var document = new PrintDocument();

            document.PrintPage += PrintDocument;

            await Dialogs.Print
                .ShowDialogAsync(new PrintDialogOptions(document));
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
