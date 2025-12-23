using System.Collections.Generic;
using System.Collections.Immutable;

using ReactiveUI;

namespace Movere.ViewModels
{
    internal static class DialogActionSetViewModel
    {
        public static DialogActionSetViewModel<TContent, TResult> Create<TContent, TResult>(
            DialogActionSet<TContent, TResult> actions) =>
            new DialogActionSetViewModel<TContent, TResult>(actions);
    }

    internal sealed class DialogActionSetViewModel<TContent, TResult> : ReactiveObject
    {
        public DialogActionSetViewModel(
            DialogActionSet<TContent, TResult> actions)
        {
            var builder = ImmutableArray.CreateBuilder<DialogActionViewModel<TContent, TResult>>();

            foreach (var action in actions.Actions)
            {
                builder.Add(
                    new DialogActionViewModel<TContent, TResult>(
                        action,
                        EqualityComparer<DialogAction<TContent, TResult>?>.Default.Equals(action, actions.CancelAction),
                        EqualityComparer<DialogAction<TContent, TResult>?>.Default.Equals(action, actions.DefaultAction)
                    )
                );
            }

            Actions = builder.ToImmutable();

            CancelAction = actions.CancelAction;
            DefaultAction = actions.DefaultAction;
        }

        public ImmutableArray<DialogActionViewModel<TContent, TResult>> Actions { get; }

        public DialogAction<TContent, TResult>? CancelAction { get; }

        public DialogAction<TContent, TResult>? DefaultAction { get; }
    }
}
