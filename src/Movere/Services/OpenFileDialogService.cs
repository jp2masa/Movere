using System.Linq;
using System.Threading.Tasks;

using Window = Avalonia.Controls.Window;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class OpenFileDialogService : IOpenFileDialogService
    {
        private readonly Window _owner;

        public OpenFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<OpenFileDialogResult> ShowDialogAsync(bool allowMultipleSelection = false)
        {
            var dialog = new OpenFileDialog() { DataTemplates = { ViewResolver.Default } };

            var viewModel = new OpenFileDialogViewModel(
                new DialogView<OpenFileDialogResult>(dialog),
                ClipboardService.Instance,
                DefaultFileIconProvider.Instance,
                new MessageDialogService(dialog),
                allowMultipleSelection,
                Enumerable.Empty<FileDialogFilter>());

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<OpenFileDialogResult>(_owner);
        }
    }
}
