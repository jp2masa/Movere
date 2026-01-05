using System;

using Avalonia;

namespace Movere
{
    internal static class AvaloniaLocatorExtensions
    {
        public static T GetRequiredService<T>(this IAvaloniaDependencyResolver resolver) =>
            resolver.GetService<T>() ?? throw new InvalidOperationException();
    }
}
