using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class ContentDialogService<TContent, TResult> : IContentDialogService<TContent, TResult>
        where TContent : notnull
    {
        private readonly Window _owner;
        private readonly IDataTemplate _viewResolver;

        public ContentDialogService(Window owner, IDataTemplate viewResolver)
        {
            _owner = owner;
            _viewResolver = viewResolver;
        }

        public Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options)
        {
            var dialog = new ContentDialogView() { DataTemplates = { _viewResolver } };

            var viewModel = new ContentDialogViewModel<TContent, TResult>(
                new DialogView<TResult>(dialog),
                options.Title,
                options.Content,
                options.Actions);

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<TResult>(_owner);
        }
    }
}
