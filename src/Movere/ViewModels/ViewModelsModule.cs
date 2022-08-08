using Autofac;

namespace Movere.ViewModels
{
    internal class ViewModelsModule : Module
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
