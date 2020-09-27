using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class ContentDialogService<TContent> : IContentDialogService<TContent>
        where TContent : notnull
    {
        private readonly Window _owner;
        private readonly IDataTemplate _viewResolver;

        public ContentDialogService(Window owner, IDataTemplate viewResolver)
        {
            _owner = owner;
            _viewResolver = viewResolver;
        }

        public Task<DialogResult> ShowDialogAsync(ContentDialogOptions<TContent> options)
        {
            var dialog = new ContentDialog() { DataTemplates = { _viewResolver } };

            var viewModel = new ContentDialogViewModel(
                new DialogView<DialogResult>(_owner),
                options.Title,
                options.Content,
                options.DialogResults);

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<DialogResult>(_owner);
        }
    }
}
