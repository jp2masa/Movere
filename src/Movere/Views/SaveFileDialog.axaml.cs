﻿using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class SaveFileDialog : ReactiveUserControl<SaveFileDialogViewModel>
    {
        public SaveFileDialog()
        {
            InitializeComponent();
        }
    }
}
