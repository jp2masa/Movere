using Avalonia;
using Avalonia.Controls.Platform;

using Movere.Storage;

namespace Movere
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder.AfterSetup(RegisterMovereDialogs);

        private static void RegisterMovereDialogs(AppBuilder builder) =>
            AvaloniaLocator.CurrentMutable
                .Bind<ISystemDialogImpl>().ToSingleton<MovereSystemDialogImpl>()
                .Bind<IStorageProviderFactory>().ToSingleton<MovereStorageProviderFactory>();
    }
}
