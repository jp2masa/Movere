using Autofac;
using Module = Autofac.Module;

using Avalonia;

namespace Movere.Services
{
    internal class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .InNamespaceOf<ServicesModule>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder
                .RegisterGeneric(typeof(DialogView<>))
                .As(typeof(IDialogView<>));

            if (builder
                .Properties
                .TryGetValue("Application", out var applicationObj)
                && applicationObj is Application application
            )
            {
                var extensions = application.GetValue(AppBuilderExtensions.MovereExtensionsProperty);

                foreach (var extension in extensions)
                {
                    builder
                        .Register((_, _) => extension.Factory())
                        .As(extension.Type);
                }
            }
        }
    }
}
