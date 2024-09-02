using System;

using Autofac.Features.Indexed;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Movere.Services
{
    internal sealed class ViewResolver : IDataTemplate
    {
        private readonly IIndex<Type, Func<Control>> _index;

        public ViewResolver(IIndex<Type, Func<Control>> index)
        {
            _index = index;
        }

        public bool Match(object? data) =>
            GetFactory(data) is not null;

        public Control? Build(object? param) =>
            GetFactory(param)?.Invoke()
                ?? throw new NotSupportedException();

        private Func<Control>? GetFactory(object? vm) =>
            vm is not null
            && (
                _index.TryGetValue(vm.GetType(), out var factory)
                || (
                    vm.GetType().IsConstructedGenericType
                    && _index.TryGetValue(vm.GetType().GetGenericTypeDefinition(), out factory)
                )
            )
                ? factory
                : null;
    }
}
