﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Movere.ViewModels;

namespace Movere.Views
{
    internal sealed partial class FileExplorerTreeView : ReactiveUserControl<FileExplorerTreeViewModel>
    {
        public FileExplorerTreeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
