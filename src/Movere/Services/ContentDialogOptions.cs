using Movere.Models;
using Movere.ViewModels;

namespace Movere.Services
{
    public sealed record ContentDialogOptions<TContent, TResult>(
        LocalizedString Title,
        TContent Content,
        DialogActionSet<TContent, TResult> Actions);

    public static class ContentDialogOptions
    {
        public static ContentDialogOptions<TContent, TResult> Create<TContent, TResult>(
            LocalizedString title,
            TContent content,
            DialogActionSet<TContent, TResult> actions) =>
            new ContentDialogOptions<TContent, TResult>(title, content, actions);
    }
}
