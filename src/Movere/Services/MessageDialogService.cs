using System.Threading.Tasks;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class MessageDialogService : IMessageDialogService
    {
        private readonly Window _owner;

        public MessageDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<IDialogResult?> ShowMessageDialogAsync(MessageDialogOptions options)
        {
            var dialog = new MessageDialog();
            var viewModel = new MessageDialogViewModel(new DialogView<IDialogResult>(dialog), options);

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<IDialogResult>(_owner);
        }
    }
}
