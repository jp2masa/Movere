using System;

using Autofac;

using Avalonia.Controls;
using Avalonia.ReactiveUI;

namespace Movere.Views
{
    internal class ViewsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .InNamespaceOf<ViewsModule>()
                .Where(x => GetViewModelType(x) is not null)
                .Keyed<Control>(x => GetViewModelType(x)!);
        }

        private static Type? GetViewModelType(Type viewType)
        {
            var type = viewType;

            while (type is not null)
            {
                if (type.IsGenericType)
                {
                    var definition = type.GetGenericTypeDefinition();

                    if (definition == typeof(ReactiveWindow<>)
                        || definition == typeof(ReactiveUserControl<>))
                    {
                        return type.GenericTypeArguments[0];
                    }
                }

                type = type.BaseType;
            }

            return null;
        }
    }
}
