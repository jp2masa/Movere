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
            var dialog = new OpenFileDialog();

            var viewModel = new OpenFileDialogViewModel(
                new DialogView<OpenFileDialogResult>(dialog),
                allowMultipleSelection,
                Enumerable.Empty<FileDialogFilter>(),
                new MessageDialogService(dialog));

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<OpenFileDialogResult>(_owner);
        }
    }
}
