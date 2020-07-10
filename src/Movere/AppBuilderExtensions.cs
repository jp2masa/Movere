using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;

namespace Movere
{
    public static class AppBuilderExtensions
    {
        public static TAppBuilder UseMovere<TAppBuilder>(this AppBuilderBase<TAppBuilder> builder)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new() =>
            builder.AfterSetup(RegisterMovereDialogs);

        private static void RegisterMovereDialogs<TAppBuilder>(AppBuilderBase<TAppBuilder> builder)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new() =>
            AvaloniaLocator.CurrentMutable.Bind<ISystemDialogImpl>().ToSingleton<MovereSystemDialogImpl>();
    }
}
