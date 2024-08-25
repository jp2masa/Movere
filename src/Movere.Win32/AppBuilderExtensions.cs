using System;
using System.Runtime.Versioning;

using Avalonia;

using Movere.Services;
using Movere.Win32.Services;

namespace Movere.Win32
{
    public static class AppBuilderExtensions
    {
        [SupportedOSPlatform("windows5.1.2600")]
        private static Movere.AppBuilderExtensions.Extension s_extension =
            new Movere.AppBuilderExtensions.Extension(typeof(IFileIconProvider), () => new FileIconProvider());

        public static AppBuilder UseMovereWin32(this AppBuilder builder) =>
            builder.AfterSetup(
                x =>
                {
                    if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600, 2600))
                    {
                        x.Instance?.AddExtension(s_extension);
                    }
                }
            );
    }
}
