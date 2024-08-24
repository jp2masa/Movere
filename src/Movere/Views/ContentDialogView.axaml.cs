﻿using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class ContentDialogView : ReactiveUserControl<IContentDialogViewModel>
    {
        public ContentDialogView()
        {
            InitializeComponent();
        }
    }
}
