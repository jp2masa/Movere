using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Autofac;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Services;
using Movere.ViewModels;

namespace Movere.Avalonia.Services
{
    public abstract class DialogHostBase : IDialogHost
    {
        protected abstract class AvaloniaDialogViewBase<TResult>
        {
            public abstract Control Root { get; }

            public abstract DialogHostBase Host { get; }

            public abstract IObservable<TResult> Show();

            public abstract void Close(TResult result);
        }

        private sealed class DialogView<TResult>(
            AvaloniaDialogViewBase<TResult> inner,
            ILifetimeScope lifetimeScope
        )
            : IMovereDialogView<TResult>
        {
            public IDialogHost Host =>
                inner.Host;

            ILifetimeScope IMovereDialogView<TResult>.LifetimeScope =>
                lifetimeScope;

            public IObservable<TResult> Show() =>
                inner.Show();

            public void Close(TResult result)
            {
                inner.Close(result);
                lifetimeScope.Dispose();
            }
        }

        private readonly IContainer _container;

        private protected DialogHostBase(Application application, IDataTemplate? dataTemplate = null)
        {
            Application = application;
            DataTemplate = dataTemplate;

            var containerBuilder = new ContainerBuilder()
            {
                Properties =
                {
                    ["Application"] = Application
                }
            };

            containerBuilder
                .RegisterAssemblyModules(typeof(DialogHostBase).Assembly);

            _container = containerBuilder.Build();
        }

        public Application Application { get; }

        public IDataTemplate? DataTemplate { get; }

        public IObservable<TResult> ShowDialog<TContent, TResult>(
            Func<IDialogView<TResult>, IDialogWindowViewModel<TContent>> viewModelFactory
        )
        {
            var inner = CreateAvaloniaDialogView<TResult>();

            if (inner is null)
            {
                return Observable.Create<TResult>(
                    observer =>
                    {
                        observer.OnError(new InvalidOperationException());
                        return Disposable.Empty;
                    }
                );
            }

            var lifetimeScope = _container.BeginLifetimeScope(
                x => x.RegisterInstance<IDialogHost>(inner.Host)
            );

            var view = new DialogView<TResult>(inner, lifetimeScope);
            var resolver = lifetimeScope.Resolve<ViewResolver>();

            var dataTemplate = DataTemplate is null
                ? (IDataTemplate)resolver
                : new FuncDataTemplate(
                    x => DataTemplate.Match(x) || resolver.Match(x),
                    (x, _) => DataTemplate.Match(x) ? DataTemplate.Build(x) : resolver.Build(x)
                );

            inner.Root.DataTemplates.Add(dataTemplate);
            inner.Root.DataContext = viewModelFactory(view);

            return view.Show();
        }

        protected abstract AvaloniaDialogViewBase<TResult>? CreateAvaloniaDialogView<TResult>();

        public ValueTask DisposeAsync() =>
            _container.DisposeAsync();
    }
}
