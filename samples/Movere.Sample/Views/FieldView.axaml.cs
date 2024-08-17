﻿using Avalonia.ReactiveUI;

using Movere.Sample.ViewModels;

namespace Movere.Sample.Views
{
    internal sealed partial class FieldView : ReactiveUserControl<FieldViewModel>
    {
        public FieldView()
        {
            InitializeComponent();
        }
    }
}
