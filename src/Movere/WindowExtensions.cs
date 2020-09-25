using System;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;

using Win32WindowImpl = Avalonia.Win32.WindowImpl;

namespace Movere
{
    internal static class WindowExtensions
    {
        private static readonly MethodInfo s_getExtendedStyle = typeof(Win32WindowImpl).GetMethod("GetExtendedStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo s_setExtendedStyle = typeof(Win32WindowImpl).GetMethod("SetExtendedStyle", BindingFlags.NonPublic | BindingFlags.Instance);

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
            if (window.PlatformImpl is Win32WindowImpl win && e.NewValue is bool enableWin32DialogModalFrame)
            {
                var exStyle = (WindowStyles)s_getExtendedStyle.Invoke(win, null);

                if (enableWin32DialogModalFrame)
                {
                    exStyle |= WindowStyles.WS_EX_DLGMODALFRAME;
                }
                else
                {
                    exStyle &= ~WindowStyles.WS_EX_DLGMODALFRAME;
                }

                s_setExtendedStyle.Invoke(win, new object[] { exStyle, true });
            }
        }

        [Flags]
        private enum WindowStyles : uint
        {
            WS_EX_DLGMODALFRAME = 0x00000001
        }
    }
}
