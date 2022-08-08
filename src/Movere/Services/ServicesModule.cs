using Autofac;

using Movere.Services;

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
       }
    }
}
