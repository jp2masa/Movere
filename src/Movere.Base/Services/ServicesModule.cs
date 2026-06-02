using Autofac;

namespace Movere.Services
{
    internal sealed class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .InNamespaceOf<ServicesModule>()
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}
