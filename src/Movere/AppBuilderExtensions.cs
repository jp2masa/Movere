using System;

using Avalonia;
using Avalonia.Controls.Platform;

namespace Movere
{
    public static class AppBuilderExtensions
    {
        [Obsolete]
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder.AfterSetup(RegisterMovereDialogs);

        [Obsolete]
        private static void RegisterMovereDialogs(AppBuilder builder) =>
            AvaloniaLocator.CurrentMutable.Bind<ISystemDialogImpl>().ToSingleton<MovereSystemDialogImpl>();
    }
}
