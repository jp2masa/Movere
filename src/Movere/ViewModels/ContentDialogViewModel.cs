using System.Collections.Generic;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.ViewModels
{
    public sealed class ContentDialogViewModel : ReactiveObject
    {
        private readonly IDialogView<DialogResult> _view;

        internal ContentDialogViewModel(
            IDialogView<DialogResult> view,
            string title,
            object content,
            IReadOnlyList<DialogResult> dialogResults)
        {
            _view = view;

            Title = title;
            Content = content;

            DialogResults = dialogResults;
        }

        public string Title { get; }

        public object Content { get; }

        public IReadOnlyList<DialogResult> DialogResults { get; }
    }
}
