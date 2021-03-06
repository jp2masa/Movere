﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed class MessageDialogView : ReactiveUserControl<MessageDialogViewModel>
    {
        public MessageDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
