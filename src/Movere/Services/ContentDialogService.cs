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

            _viewResolver = new FuncDataTemplate(
                x => x is IContentDialogViewModel || viewResolver.Match(x),
                (x, _) => x is IContentDialogViewModel
                    ? new ContentDialogView()
                    : viewResolver.Build(x)
            );
        }

        public Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options)
        {
            var window = new DialogWindow() { DataTemplates = { _viewResolver } };
            var dialogView = new DialogView<TResult>(window);

            var viewModel = ContentDialogViewModel.Create(options.Content, options.Actions);

            window.DataContext = InternalDialogWindowViewModel.Create(dialogView, options.Title, viewModel);

            return window.ShowDialog<TResult>(_owner);
        }
    }
}
