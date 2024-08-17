﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class FileExplorerFolderView : ReactiveUserControl<FileExplorerFolderViewModel>
    {
        public FileExplorerFolderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
