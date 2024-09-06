using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowMessageDialog* extension methods on IDialogHost instead")]
    public interface IMessageDialogService
    {
        Task<DialogResult> ShowMessageDialogAsync(MessageDialogOptions options);
    }
}
