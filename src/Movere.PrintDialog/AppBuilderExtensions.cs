using Autofac;

using Avalonia;

namespace Movere.PrintDialog
{
    public static class AppBuilderExtensions
    {
        private static readonly Movere.AppBuilderExtensions.Extension s_extension =
            new Movere.AppBuilderExtensions.Extension(x => x.RegisterAssemblyModules(typeof(AppBuilderExtensions).Assembly));

        public static AppBuilder UseMoverePrintDialogs(this AppBuilder builder) =>
            builder.AfterSetup(x => x.Instance?.AddExtension(s_extension));
    }
}
