using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowSaveFileDialog* extension methods on IDialogHost instead")]
    public interface ISaveFileDialogService
    {
        Task<SaveFileDialogResult> ShowDialogAsync(SaveFileDialogOptions? options = null);
    }
}
