using System.Collections.Generic;

namespace Movere.Models
{
    public sealed class ContentDialogOptions<TContent>
    {
        public ContentDialogOptions(
            string title,
            TContent content,
            IReadOnlyList<DialogResult>? dialogResults = null)
        {
            Title = title;
            Content = content;
            DialogResults = dialogResults ?? DialogResultSet.OK;
        }

        public string Title { get; }

        public TContent Content { get; }

        public IReadOnlyList<DialogResult> DialogResults { get; }
    }

    public static class ContentDialogOptions
    {
        public static ContentDialogOptions<TContent> Create<TContent>(string title, TContent content, IReadOnlyList<DialogResult>? dialogResults = null) =>
            new ContentDialogOptions<TContent>(title, content, dialogResults);
    }
}
