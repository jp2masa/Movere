using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Sample.ViewModels;
using Movere.Sample.Views;

namespace Movere.Sample
{
    internal sealed class CustomContentViewResolver : IDataTemplate
    {
        public bool Match(object? data) => data is CustomContentViewModel || data is FieldViewModel;

        public Control? Build(object? param) =>
            param switch
            {
                CustomContentViewModel => new CustomContentView(),
                FieldViewModel => new FieldView(),
                _ => throw new InvalidOperationException()
            };
    }
}
