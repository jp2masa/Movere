﻿using Avalonia.Markup.Xaml;

using Movere.Controls;

namespace Movere.Views
{
    public sealed partial class GridFolderView : ItemsControlView
    {
        public GridFolderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
