using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

using Movere.Models;

namespace Movere.Converters
{
    internal sealed class FileExplorerTreeIsExpandedConverter : IMultiValueConverter
    {
        public static readonly FileExplorerTreeIsExpandedConverter Instance = new FileExplorerTreeIsExpandedConverter();

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v is BindingNotification notification && notification.ErrorType == BindingErrorType.Error))
            {
                return AvaloniaProperty.UnsetValue;
            }

            if (values.Count == 2 && values[0] is Folder itemFolder && targetType == typeof(bool))
            {
                if (values[1] is Folder selectedFolder)
                {
                    if (selectedFolder.Parent != null
                        && selectedFolder.Parent.FullPath.StartsWith(itemFolder.FullPath))
                    {
                        return true;
                    }
                }

                return BindingOperations.DoNothing;
            }

            throw new NotSupportedException();
        }
    }
}
