using System;
using System.Collections.Immutable;

using Autofac;

using Avalonia;

namespace Movere
{
    public static partial class AppBuilderExtensions
    {
        internal static readonly AttachedProperty<ImmutableArray<Extension>> MovereExtensionsProperty =
            AvaloniaProperty.RegisterAttached<Application, ImmutableArray<Extension>>(
                "MovereExtensions",
                typeof(AppBuilderExtensions),
                ImmutableArray<Extension>.Empty
            );

        internal sealed record Extension(Action<ContainerBuilder> RegisterExtension);

        internal static void AddExtension(this Application application, Extension extension) =>
            application.SetValue(
                MovereExtensionsProperty,
                application
                    .GetValue(MovereExtensionsProperty)
                    .Add(extension)
            );
    }
}
