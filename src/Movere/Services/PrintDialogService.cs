using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Threading.Tasks;

using Avalonia.Controls;

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

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
        public Task<bool> ShowDialogAsync(PrintDocument document)
        {
            var dialog = new PrintDialog();
            var viewModel = new PrintDialogViewModel(document, r => dialog.Close(r));

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<bool>(_owner);
        }
    }
}
