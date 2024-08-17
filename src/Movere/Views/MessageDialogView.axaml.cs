﻿using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class MessageDialogView : ReactiveUserControl<MessageDialogViewModel>
    {
        public MessageDialogView()
        {
            InitializeComponent();
        }
    }
}
