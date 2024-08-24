using System;

using Avalonia;
using Avalonia.Controls;

namespace Movere
{
    internal static class WindowExtensions
    {
        public static readonly AttachedProperty<bool> EnableWin32DialogModalFrameProperty =
            AvaloniaProperty.RegisterAttached<Window, bool>("EnableWin32DialogModalFrame", typeof(WindowExtensions));

        static WindowExtensions()
        {
            EnableWin32DialogModalFrameProperty.Changed.AddClassHandler<Window>(OnEnableWin32DialogModalFramePropertyChanged);
        }

        public static bool GetEnableWin32DialogModalFrame(Window window) =>
            window.GetValue(EnableWin32DialogModalFrameProperty);

        public static void SetEnableWin32DialogModalFrame(Window window, bool value) =>
            window.SetValue(EnableWin32DialogModalFrameProperty, value);

        private static void OnEnableWin32DialogModalFramePropertyChanged(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is not bool enableWin32DialogModalFrame)
            {
                return;
            }

            if (enableWin32DialogModalFrame)
            {
                Win32Properties.AddWindowStylesCallback(window, WindowStylesCallback);
            }
            else
            {
                Win32Properties.RemoveWindowStylesCallback(window, WindowStylesCallback);
            }
        }

        private static (uint style, uint exStyle) WindowStylesCallback(uint style, uint exStyle) =>
            (style, exStyle | (uint)WindowStyles.WS_EX_DLGMODALFRAME);

        [Flags]
        private enum WindowStyles : uint
        {
            WS_EX_DLGMODALFRAME = 0x00000001
        }
    }
}
