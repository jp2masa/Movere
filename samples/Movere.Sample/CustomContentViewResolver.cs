﻿using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Movere.Sample.ViewModels;
using Movere.Sample.Views;

namespace Movere.Sample
{
    class CustomContentViewResolver : IDataTemplate
    {
        public bool Match(object data) => data is CustomContentViewModel || data is FieldViewModel;

        public IControl Build(object param) =>
            param switch
            {
                CustomContentViewModel _ => new CustomContentView(),
                FieldViewModel _ => new FieldView(),
                _ => throw new InvalidOperationException()
            };
    }
}
