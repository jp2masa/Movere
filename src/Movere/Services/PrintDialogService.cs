using System.Threading.Tasks;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class PrintDialogService : IPrintDialogService
    {
        private readonly Window _owner;

        public PrintDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<bool> ShowDialogAsync(PrintDialogOptions options)
        {
            var dialog = new PrintDialog();
            var viewModel = new PrintDialogViewModel(options, r => dialog.Close(r));

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<bool>(_owner);
        }
    }
}
