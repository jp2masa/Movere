using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowOpenFileDialog* extension methods on IDialogHost instead")]
    public interface IOpenFileDialogService
    {
        Task<OpenFileDialogResult> ShowDialogAsync(OpenFileDialogOptions? options = null);
    }
}
