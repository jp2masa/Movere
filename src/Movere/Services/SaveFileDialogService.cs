using System.Threading.Tasks;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using SaveFileDialog = Movere.Views.SaveFileDialog;

namespace Movere.Services
{
    public sealed class SaveFileDialogService : ISaveFileDialogService
    {
        private readonly Window _owner;

        public SaveFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<SaveFileDialogResult> ShowDialogAsync()
        {
            var dialog = new SaveFileDialog();

            var viewModel = new SaveFileDialogViewModel(
                new DialogView<SaveFileDialogResult>(dialog),
                new MessageDialogService(dialog));

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<SaveFileDialogResult>(_owner);
        }
    }
}
