using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowPrintDialog* extension methods on IDialogHost instead")]
    public sealed class PrintDialogService(IDialogHost host) : IPrintDialogService
    {
        public Task<bool> ShowDialogAsync(PrintDialogOptions options) =>
            host.ShowPrintDialogAsync(options);
    }
}
