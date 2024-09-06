using System;
using System.Threading.Tasks;

using Movere.Models;

namespace Movere.Services
{
    [Obsolete("Use .ShowPrintDialog* extension methods on IDialogHost instead")]
    public interface IPrintDialogService
    {
        Task<bool> ShowDialogAsync(PrintDialogOptions options);
    }
}
