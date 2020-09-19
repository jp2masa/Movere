using System;
using System.Collections.Immutable;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    internal sealed class ViewResolver : IDataTemplate
    {
        public static ViewResolver Default { get; } = new ViewResolver();

        private static readonly IImmutableDictionary<Type, Func<IControl>> s_mappings =
            ImmutableDictionary.Create<Type, Func<IControl>>()
                               .Add(typeof(FileExplorerAddressBarViewModel), New<FileExplorerAddressBarView>)
                               .Add(typeof(FileExplorerFolderViewModel), New<FileExplorerFolderView>)
                               .Add(typeof(FileExplorerTreeViewModel), New<FileExplorerTreeView>)
                               .Add(typeof(FileExplorerViewModel), New<FileExplorerView>);

        private ViewResolver() { }

        public bool Match(object data) => !(data is null) && s_mappings.ContainsKey(data.GetType());

        public IControl Build(object param) => s_mappings[param.GetType()]();

        private static T New<T>() where T : new() => new T();
    }
}
