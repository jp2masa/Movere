﻿// https://github.com/AvaloniaUI/Avalonia/blob/82d64089e15dca3712dc87dce757a29ccef2a04e/src/Avalonia.Base/Compatibility/OperatingSystem.cs

#if NET6_0_OR_GREATER
using System;
#else
using System.Runtime.InteropServices;
#endif

namespace Movere.Storage
{
    internal sealed class OperatingSystemEx
    {
#if NET6_0_OR_GREATER
        public static bool IsWindows() => OperatingSystem.IsWindows();
        public static bool IsMacOS() => OperatingSystem.IsMacOS();
        public static bool IsMacCatalyst() => OperatingSystem.IsMacCatalyst();
        public static bool IsLinux() => OperatingSystem.IsLinux();
        public static bool IsFreeBSD() => OperatingSystem.IsFreeBSD();
        public static bool IsAndroid() => OperatingSystem.IsAndroid();
        public static bool IsIOS() => OperatingSystem.IsIOS();
        public static bool IsTvOS() => OperatingSystem.IsTvOS();
        public static bool IsBrowser() => OperatingSystem.IsBrowser();
        public static bool IsOSPlatform(string platform) => OperatingSystem.IsOSPlatform(platform);
#else
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsFreeBSD() => false;
        public static bool IsAndroid() => false;
        public static bool IsIOS() => false;
        public static bool IsMacCatalyst() => false;
        public static bool IsTvOS() => false;
        public static bool IsBrowser() => false;
        public static bool IsOSPlatform(string platform) => RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform));
#endif
    }
}
