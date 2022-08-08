using System;

using Autofac.Features.Indexed;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Movere.Services
{
    internal sealed class ViewResolver : IDataTemplate
    {
        private readonly IIndex<Type, Func<IControl>> _index;

        public ViewResolver(IIndex<Type, Func<IControl>> index)
        {
            _index = index;
        }

        public bool Match(object data) =>
            data is not null && _index.TryGetValue(data.GetType(), out _);

        public IControl Build(object param) =>
            param is not null && _index.TryGetValue(param.GetType(), out var factory)
                ? factory()
                : throw new NotSupportedException();
    }
}
