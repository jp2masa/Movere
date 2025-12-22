using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

using ReactiveUI.Avalonia;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class DialogOverlay
        : ReactiveUserControl<IDialogWindowViewModel>, IFocusScope
    {
        private static readonly AttachedProperty<IList<DialogOverlay>> DialogOverlaysProperty =
            AvaloniaProperty.RegisterAttached<DialogOverlay, AvaloniaObject, IList<DialogOverlay>>(
                "DialogOverlays"
            );

        private static readonly AttachedProperty<DialogOverlay?> RootDialogOverlayProperty =
            AvaloniaProperty.RegisterAttached<DialogOverlay, AvaloniaObject, DialogOverlay?>(
                "RootDialogOverlay",
                inherits: true
            );

        private Button? _cancelButton;
        private Button? _defaultButton;

        static DialogOverlay()
        {
            ClipToBoundsProperty.OverrideDefaultValue<DialogOverlay>(false);

            KeyboardNavigation.TabNavigationProperty
                .OverrideDefaultValue<DialogOverlay>(KeyboardNavigationMode.Cycle);

            RootDialogOverlayProperty.Changed.AddClassHandler<Button>(
                static (sender, e) =>
                {
                    if (e.OldValue is DialogOverlay oldDialogOverlay)
                    {
                        if (sender.IsCancel)
                        {
                            oldDialogOverlay._cancelButton = null;
                        }

                        if (sender.IsDefault)
                        {
                            oldDialogOverlay._defaultButton = null;
                        }
                    }

                    if (e.NewValue is DialogOverlay newDialogOverlay)
                    {
                        if (sender.IsCancel)
                        {
                            newDialogOverlay._cancelButton = sender;
                        }

                        if (sender.IsDefault)
                        {
                            newDialogOverlay._defaultButton = sender;
                        }
                    }
                }
            );

            Button.IsCancelProperty.Changed.AddClassHandler<Button>(
                static (sender, e) =>
                {
                    if (sender.GetValue(RootDialogOverlayProperty) is not { } dialogOverlay)
                    {
                        return;
                    }

                    if (e.NewValue is bool newValue)
                    {
                        if (newValue)
                        {
                            dialogOverlay._cancelButton = sender;
                        }
                        else if (Object.ReferenceEquals(dialogOverlay._cancelButton, sender))
                        {
                            dialogOverlay._cancelButton = null;
                        }
                    }
                }
            );

            Button.IsDefaultProperty.Changed.AddClassHandler<Button>(
                static (sender, e) =>
                {
                    if (sender.GetValue(RootDialogOverlayProperty) is not { } dialogOverlay)
                    {
                        return;
                    }

                    if (e.NewValue is bool newValue)
                    {
                        if (newValue)
                        {
                            dialogOverlay._defaultButton = sender;
                        }
                        else if (Object.ReferenceEquals(dialogOverlay._defaultButton, sender))
                        {
                            dialogOverlay._defaultButton = null;
                        }
                    }
                }
            );
        }

        public DialogOverlay()
        {
            InitializeComponent();
        }

        public bool OnClosing() =>
            ViewModel?.OnClosing() ?? true;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            SetCurrentValue(RootDialogOverlayProperty, this);

            if (e.Root is IInputElement ie
                && e.Root is AvaloniaObject obj)
            {
                var overlays = obj.GetValue(DialogOverlaysProperty)
                    ?? new AvaloniaList<DialogOverlay>();

                overlays.Add(this);
                obj.SetValue(DialogOverlaysProperty, overlays);

                ie.AddHandler(KeyDownEvent, HandleKeyDown);
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if (e.Root is IInputElement ie
                && e.Root is AvaloniaObject obj)
            {
                ie.RemoveHandler(KeyDownEvent, HandleKeyDown);

                var overlays = obj.GetValue(DialogOverlaysProperty)
                    ?? new AvaloniaList<DialogOverlay>();

                overlays.Remove(this);
                obj.SetValue(DialogOverlaysProperty, overlays);
            }

            SetCurrentValue(RootDialogOverlayProperty, null);

            base.OnDetachedFromVisualTree(e);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = base.MeasureOverride(availableSize);

            return new Size(
                Math.Max(availableSize.Width, desiredSize.Width),
                Math.Max(availableSize.Height, desiredSize.Height)
            );
        }

        private static void HandleKeyDown(object? sender, KeyEventArgs e)
        {
            if (sender is not AvaloniaObject obj
                || obj.GetValue(DialogOverlaysProperty) is not { } overlays)
            {
                return;
            }

            if (e.Key == Key.Escape)
            {
                var cancelButton = overlays
                    .LastOrDefault(static x => x._cancelButton is not null)
                    ?._cancelButton;

                if (cancelButton is not null)
                {
                    e.Handled = ButtonClick(cancelButton);
                }
            }
            else if (e.Key == Key.Enter)
            {
                var defaultButton = overlays
                    .LastOrDefault(static x => x._defaultButton is not null)
                    ?._defaultButton;

                if (defaultButton is not null)
                {
                    e.Handled = ButtonClick(defaultButton);
                }
            }
        }

        // adapted from https://github.com/AvaloniaUI/Avalonia/blob/d5f0188ccf9e36d9a3a6207bf4054deadf064f60/src/Avalonia.Controls/Button.cs#L333-L356
        private static bool ButtonClick(Button button)
        {
            if (!button.IsEffectivelyEnabled)
            {
                return false;
            }

            var e = new RoutedEventArgs(Button.ClickEvent);
            button.RaiseEvent(e);

            var (command, parameter) = (button.Command, button.CommandParameter);

            if (!e.Handled && command is not null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
