using System;
using System.Threading.Tasks;

using Autofac;

using Avalonia;
using Avalonia.Controls.Templates;

using Movere.Services;
using Movere.ViewModels;

namespace Movere.Avalonia.Services
{
    public abstract class DialogHostBase : IMovereDialogHost
    {
        private readonly IContainer _container;

        private protected DialogHostBase(Application application, IDataTemplate? dataTemplate = null)
        {
            var containerBuilder = new ContainerBuilder()
            {
                Properties =
                {
                    ["Application"] = application
                }
            };

            containerBuilder
                .RegisterAssemblyModules(typeof(WindowDialogHost).Assembly);

            containerBuilder
                .RegisterInstance<IDialogHost>(this);

            _container = containerBuilder.Build();

            var resolver = _container.Resolve<ViewResolver>();

            DataTemplate = dataTemplate is null
                ? resolver
                : new FuncDataTemplate(
                    x => dataTemplate.Match(x) || resolver.Match(x),
                    (x, _) => dataTemplate.Match(x) ? dataTemplate.Build(x) : resolver.Build(x)
                );
        }

        protected IDataTemplate DataTemplate { get; }

        IContainer IMovereDialogHost.Container =>
            _container;

        public abstract IObservable<TResult> ShowDialog<TContent, TResult>(
            Func<IDialogView<TResult>, IDialogWindowViewModel<TContent>> viewModelFactory
        );

        public ValueTask DisposeAsync() =>
            _container.DisposeAsync();
    }
}
