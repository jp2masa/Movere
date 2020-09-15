using System.Collections.Generic;

namespace Movere.Models
{
    public static class DialogResultSet
    {
        public static IReadOnlyList<DialogResult> AbortRetryIgnore { get; } = new DialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };

        public static IReadOnlyList<DialogResult> OK { get; } = new DialogResult[] { DialogResult.OK };

        public static IReadOnlyList<DialogResult> OKCancel { get; } = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };

        public static IReadOnlyList<DialogResult> RetryCancel { get; } = new DialogResult[] { DialogResult.Retry, DialogResult.Cancel };

        public static IReadOnlyList<DialogResult> YesNo { get; } = new DialogResult[] { DialogResult.Yes, DialogResult.No };

        public static IReadOnlyList<DialogResult> YesNoCancel { get; } = new DialogResult[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
    }
}
