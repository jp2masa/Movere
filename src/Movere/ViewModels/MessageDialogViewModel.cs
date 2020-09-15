using System.Collections.Generic;
using System.Windows.Input;

using Avalonia.Media.Imaging;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.ViewModels
{
    internal sealed class MessageDialogViewModel
    {
        private readonly IDialogView<DialogResult> _view;

        private readonly MessageDialogOptions _options;

        public MessageDialogViewModel(IDialogView<DialogResult> view, MessageDialogOptions options)
        {
            _view = view;

            _options = options;

            CloseCommand = ReactiveCommand.Create<DialogResult>(Close);
        }

        public IBitmap? Icon => _options.Icon.LoadIcon();

        public string Title => _options.Title;

        public string Message => _options.Message;

        public IReadOnlyList<DialogResult> DialogResults => _options.DialogResults;

        public ICommand CloseCommand { get; }

        private void Close(DialogResult result) => _view.Close(result);
    }
}
