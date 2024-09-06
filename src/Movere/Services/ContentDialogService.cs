using System;
using System.Threading.Tasks;

namespace Movere.Services
{
    [Obsolete("Use .ShowContentDialog* extension methods on IDialogHost instead")]
    public sealed class ContentDialogService<TContent, TResult>(IDialogHost host)
        : IContentDialogService<TContent, TResult>
        where TContent : notnull
    {
        public Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options) =>
            host.ShowContentDialogAsync(options);
    }
}
