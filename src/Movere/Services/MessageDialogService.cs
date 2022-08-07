using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class MessageDialogService : IMessageDialogService
    {
        private static readonly IDataTemplate s_ViewResolver = new FuncDataTemplate<MessageDialogViewModel>((vm, ns) => new MessageDialogView());

        private readonly ContentDialogService<MessageDialogViewModel, DialogResult> _contentDialogService;

        public MessageDialogService(Window owner)
        {
            _contentDialogService = new ContentDialogService<MessageDialogViewModel, DialogResult>(owner, s_ViewResolver);
        }

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
