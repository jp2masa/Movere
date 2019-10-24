using System;
using System.Collections.Generic;
using System.Windows.Input;

using Avalonia.Media.Imaging;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class MessageDialogViewModel
    {
        private readonly MessageDialogOptions _options;

        public MessageDialogViewModel(MessageDialogOptions options, Action<IDialogResult> closeAction)
        {
            _options = options;

            CloseCommand = ReactiveCommand.Create(closeAction);
        }

        public IBitmap? Icon => _options.Icon.LoadIcon();

        public string Title => _options.Title;

        public string Message => _options.Message;

        public IReadOnlyList<IDialogResult> DialogResults => _options.DialogResults;

        public ICommand CloseCommand { get; }
    }
}
