using Avalonia;

using Movere.Services;
using Movere.Win32.Services;

namespace Movere.Win32
{
    public static class AppBuilderExtensions
    {

        public static AppBuilder UseMovereWin32(this AppBuilder builder) =>
            builder.AfterSetup(
                x => x.Instance?
                    .AddExtension(
                        new Movere.AppBuilderExtensions.Extension(
                            typeof(IFileIconProvider),
                            () => new FileIconProvider()
                        )
                    )
                );
    }
}
