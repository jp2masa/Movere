using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public static class DialogAction
    {
        public static DialogAction<TContent, TResult> Create<TContent, TResult>(
            LocalizedString title,
            ReactiveCommand<TContent, TResult> command) =>
            new DialogAction<TContent, TResult>(title, command);
    }

    public sealed record DialogAction<TContent, TResult>(LocalizedString Title, ReactiveCommand<TContent, TResult> Command);

    public static class DialogActionSet
    {
        public static DialogActionSet<TContent, TResult> Create<TContent, TResult>(
            ImmutableArray<DialogAction<TContent, TResult>> actions,
            DialogAction<TContent, TResult>? cancelAction,
            DialogAction<TContent, TResult>? defaultAction) =>
            new DialogActionSet<TContent, TResult>(actions, cancelAction, defaultAction);

        public static DialogActionSet<TContent, DialogResult> FromDialogResultSet<TContent>(DialogResultSet dialogResults)
        {
            var builder = ImmutableArray.CreateBuilder<DialogAction<TContent, DialogResult>>();

            var cancelAction = default(DialogAction<TContent, DialogResult>);
            var defaultAction = default(DialogAction<TContent, DialogResult>);

            foreach (var result in dialogResults)
            {
                var action = new DialogAction<TContent, DialogResult>(
                    result.Name, ReactiveCommand.Create((TContent x) => result));

                builder.Add(action);

                cancelAction = EqualityComparer<DialogResult>.Default.Equals(result, dialogResults.CancelResult)
                    ? action
                    : cancelAction;

                defaultAction = EqualityComparer<DialogResult>.Default.Equals(result, dialogResults.DefaultResult)
                    ? action
                    : cancelAction;
            }

            return Create(builder.ToImmutable(), cancelAction, defaultAction);
        }
    }

    public sealed record DialogActionSet<TContent, TResult>
    {
        public DialogActionSet(
            ImmutableArray<DialogAction<TContent, TResult>> actions,
            DialogAction<TContent, TResult>? cancelAction = null,
            DialogAction<TContent, TResult>? defaultAction = null)
        {
            Contract.Assert(cancelAction is null || actions.Contains(cancelAction));
            Contract.Assert(defaultAction is null || actions.Contains(defaultAction));

            Actions = actions;

            CancelAction = cancelAction;
            DefaultAction = defaultAction;
        }

        public ImmutableArray<DialogAction<TContent, TResult>> Actions { get; }

        public DialogAction<TContent, TResult>? CancelAction { get; }

        public DialogAction<TContent, TResult>? DefaultAction { get; }
    }

    internal interface IDialogActionViewModel
    {
        LocalizedString Title { get; }

        IReactiveCommand Command { get; }

        bool IsCancel { get; }

        bool IsDefault { get; }
    }

    internal static class DialogActionViewModel
    {
        public static DialogActionViewModel<TContent, TResult> Create<TContent, TResult>(
            DialogAction<TContent, TResult> action,
            bool isCancel,
            bool isDefault) =>
            new DialogActionViewModel<TContent, TResult>(action, isCancel, isDefault);
    }

    internal sealed class DialogActionViewModel<TContent, TResult> : ReactiveObject, IDialogActionViewModel
    {
        public DialogActionViewModel(
            DialogAction<TContent, TResult> action,
            bool isCancel,
            bool isDefault)
        {
            Title = action.Title;
            Command = action.Command;

            IsCancel = isCancel;
            IsDefault = isDefault;
        }

        public LocalizedString Title { get; }

        public ReactiveCommand<TContent, TResult> Command { get; }

        IReactiveCommand IDialogActionViewModel.Command => Command;

        public bool IsCancel { get; }

        public bool IsDefault { get; }
    }
}
