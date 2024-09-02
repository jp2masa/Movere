using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Movere.ViewModels;

namespace Movere.Services
{
    public sealed class ContentDialogService<TContent, TResult>(IDialogHost host)
        : IContentDialogService<TContent, TResult>
        where TContent : notnull
    {
        public Task<TResult> ShowDialogAsync(ContentDialogOptions<TContent, TResult> options) =>
            host
                .ShowDialog(
                    (IDialogView<TResult> view) =>
                        InternalDialogWindowViewModel.Create(
                            view,
                            options.Title,
                            ContentDialogViewModel.Create(
                                options.Content,
                                options.Actions
                            )
                        )
                )
                .ToTask();
    }
}
