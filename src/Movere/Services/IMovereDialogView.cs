using Autofac;

namespace Movere.Services
{
    internal interface IMovereDialogView<in TResult> : IDialogView<TResult>
    {
        ILifetimeScope LifetimeScope { get; }
    }
}
