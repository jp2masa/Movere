using System;
using System.Runtime.InteropServices;

using Avalonia;
using Avalonia.Controls;

using Win32WindowImpl = Avalonia.Win32.WindowImpl;

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
            if (window.PlatformImpl is Win32WindowImpl win && e.NewValue is bool enableWin32DialogModalFrame)
            {
                var hwnd = win.Handle.Handle;
                var exStyle = (WindowStyles)GetWindowLong(hwnd, (int)WindowLongParam.GWL_EXSTYLE);

                if (enableWin32DialogModalFrame)
                {
                    exStyle |= WindowStyles.WS_EX_DLGMODALFRAME;
                }
                else
                {
                    exStyle &= ~WindowStyles.WS_EX_DLGMODALFRAME;
                }

                SetWindowLong(hwnd, (int)WindowLongParam.GWL_EXSTYLE, (uint)exStyle);
            }
        }

        // interop adapted from Avalonia

        [Flags]
        private enum WindowStyles : uint
        {
            WS_EX_DLGMODALFRAME = 0x00000001
        }

        private enum WindowLongParam
        {
            GWL_EXSTYLE = -20
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong32b(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        private static uint GetWindowLong(IntPtr hWnd, int nIndex) =>
            IntPtr.Size == 4 ? GetWindowLong32b(hWnd, nIndex) : GetWindowLongPtr(hWnd, nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLong64b(IntPtr hWnd, int nIndex, IntPtr value);

        public static uint SetWindowLong(IntPtr hWnd, int nIndex, uint value) =>
            IntPtr.Size == 4 ? SetWindowLong32b(hWnd, nIndex, value) : (uint)SetWindowLong64b(hWnd, nIndex, new IntPtr(value)).ToInt32();
    }
}
