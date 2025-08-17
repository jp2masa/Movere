﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using static Movere.Models.DialogResult;

namespace Movere.Models
{
    public sealed class DialogResultSet : IReadOnlyList<DialogResult>
    {
        public static DialogResultSet AbortRetryIgnore { get; } =
            new DialogResultSet([Abort, Retry, Ignore], Retry, Ignore);

        public static DialogResultSet OK { get; } =
            new DialogResultSet([DialogResult.OK], DialogResult.OK, DialogResult.OK);

        public static DialogResultSet OKCancel { get; } =
            new DialogResultSet([DialogResult.OK, Cancel], DialogResult.OK, Cancel);
        public static DialogResultSet RetryCancel { get; } =
            new DialogResultSet([Retry, Cancel], Retry, Cancel);

        public static DialogResultSet YesNo { get; } =
            new DialogResultSet([Yes, No], Yes, No);

        public static DialogResultSet YesNoCancel { get; } =
            new DialogResultSet([Yes, No, Cancel], Yes, Cancel);


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
