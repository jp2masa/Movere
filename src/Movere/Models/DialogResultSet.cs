using System.Collections.Generic;

namespace Movere.Models
{
    public static class DialogResultSet
    {
        public static IReadOnlyList<IDialogResult> AbortRetryIgnore { get; } = new IDialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };

        public static IReadOnlyList<IDialogResult> OK { get; } = new IDialogResult[] { DialogResult.OK };

        public static IReadOnlyList<IDialogResult> OKCancel { get; } = new IDialogResult[] { DialogResult.OK, DialogResult.Cancel };

        public static IReadOnlyList<IDialogResult> RetryCancel { get; } = new IDialogResult[] { DialogResult.Retry, DialogResult.Cancel };

        public static IReadOnlyList<IDialogResult> YesNo { get; } = new IDialogResult[] { DialogResult.Yes, DialogResult.No };

        public static IReadOnlyList<IDialogResult> YesNoCancel { get; } = new IDialogResult[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
    }
}
