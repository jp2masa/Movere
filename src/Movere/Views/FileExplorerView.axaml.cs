﻿using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    public sealed partial class FileExplorerView : ReactiveUserControl<FileExplorerViewModel>
    {
        public FileExplorerView()
        {
            InitializeComponent();
        }
    }
}
