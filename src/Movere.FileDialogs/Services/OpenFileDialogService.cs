using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowOpenFileDialog* extension methods on IDialogHost instead")]
    public sealed class OpenFileDialogService(IDialogHost host) : IOpenFileDialogService
    {
        public Task<OpenFileDialogResult> ShowDialogAsync(OpenFileDialogOptions? options = null) =>
            host.ShowOpenFileDialogAsync(options);
    }
}
