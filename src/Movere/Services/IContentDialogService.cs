using System;
using System.Threading.Tasks;

namespace Movere.Services
{
    [Obsolete("Use .ShowContentDialog* extension methods on IDialogHost instead")]
    public interface IContentDialogService<TContent, TResult>
    {
        Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options);
    }
}
