using System.Threading.Tasks;

using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public sealed class MessageDialogService(IDialogHost host) : IMessageDialogService
    {
        private readonly ContentDialogService<MessageDialogViewModel, DialogResult> _contentDialogService =
            new ContentDialogService<MessageDialogViewModel, DialogResult>(host);

        public Task<DialogResult> ShowMessageDialogAsync(MessageDialogOptions options) =>
            _contentDialogService.ShowDialogAsync(
                new ContentDialogOptions<MessageDialogViewModel, DialogResult>(
                    options.Title,
                    new MessageDialogViewModel(options.Icon, options.Message),
                    DialogActionSet.FromDialogResultSet<MessageDialogViewModel>(options.DialogResults)
                )
            );
    }
}
