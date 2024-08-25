using System;
using System.Collections.Immutable;

using Avalonia;
using Avalonia.Controls.Platform;

namespace Movere
{
    public static class AppBuilderExtensions
    {
        internal static readonly AttachedProperty<ImmutableArray<Extension>> MovereExtensionsProperty =
            AvaloniaProperty.RegisterAttached<Application, ImmutableArray<Extension>>(
                "MovereExtensions",
                typeof(AppBuilderExtensions),
                ImmutableArray<Extension>.Empty
            );

        internal sealed record Extension(Type Type, Func<object> Factory);

        internal static void AddExtension(this Application application, Extension extension) =>
            application.SetValue(
                MovereExtensionsProperty,
                application
                    .GetValue(MovereExtensionsProperty)
                    .Add(extension)
            );

        [Obsolete]
        public static AppBuilder UseMovere(this AppBuilder builder) =>
            builder.AfterSetup(RegisterMovereDialogs);

        [Obsolete]
        private static void RegisterMovereDialogs(AppBuilder builder) =>
            AvaloniaLocator.CurrentMutable.Bind<ISystemDialogImpl>().ToSingleton<MovereSystemDialogImpl>();
    }
}
