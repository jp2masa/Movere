using Autofac;

namespace Movere.ViewModels
{
    internal sealed class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .InNamespaceOf<ViewModelsModule>();
        }
    }
}
