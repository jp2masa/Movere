using Autofac;

namespace Movere.Services
{
    internal interface IMovereDialogHost : IDialogHost
    {
        IContainer Container { get; }
    }
}
