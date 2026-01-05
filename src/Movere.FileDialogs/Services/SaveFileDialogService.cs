using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowSaveFileDialog* extension methods on IDialogHost instead")]
    public sealed class SaveFileDialogService(IDialogHost host) : ISaveFileDialogService
    {
        public Task<SaveFileDialogResult> ShowDialogAsync(SaveFileDialogOptions? options = null) =>
            host.ShowSaveFileDialogAsync(options);
    }
}
