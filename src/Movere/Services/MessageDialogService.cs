using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowMessageDialog* extension methods on IDialogHost instead")]
    public sealed class MessageDialogService(IDialogHost host) : IMessageDialogService
    {
        public Task<DialogResult> ShowMessageDialogAsync(MessageDialogOptions options) =>
            host.ShowMessageDialogAsync(options);
    }
}
