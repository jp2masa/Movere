using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Movere.Models
{
    public sealed class DialogResultSet : IReadOnlyList<DialogResult>
    {
        public static DialogResultSet AbortRetryIgnore { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore),
                DialogResult.Retry,
                DialogResult.Ignore
            );

        public static DialogResultSet OK { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.OK),
                DialogResult.OK,
                DialogResult.OK
            );

        public static DialogResultSet OKCancel { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.OK, DialogResult.Cancel),
                DialogResult.OK,
                DialogResult.Cancel
            );

        public static DialogResultSet RetryCancel { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.Retry, DialogResult.Cancel),
                DialogResult.Retry,
                DialogResult.Cancel
            );

        public static DialogResultSet YesNo { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.Yes, DialogResult.No),
                DialogResult.Yes,
                DialogResult.No
            );

        public static DialogResultSet YesNoCancel { get; } =
            new DialogResultSet(
                ImmutableArray.Create(DialogResult.Yes, DialogResult.No, DialogResult.Cancel),
                DialogResult.Yes,
                DialogResult.Cancel
            );

        public DialogResultSet(ImmutableArray<DialogResult> results, DialogResult defaultResult, DialogResult cancelResult)
        {
            Results = results;

            DefaultResult = defaultResult;
            CancelResult = cancelResult;
        }
        public ImmutableArray<DialogResult> Results { get; }

        public DialogResult DefaultResult { get; }

        public DialogResult CancelResult { get; }

        public int Count => Results.Length;

        public DialogResult this[int index] => Results[index];

        public IEnumerator<DialogResult> GetEnumerator() => ((IEnumerable<DialogResult>)Results).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
