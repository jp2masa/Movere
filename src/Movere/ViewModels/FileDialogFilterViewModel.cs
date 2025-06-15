using System;
using System.Linq;
using System.Text;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class FileDialogFilterViewModel
    {
        private readonly Lazy<string> _displayText;

        public FileDialogFilterViewModel(FileDialogFilter filter)
        {
            Filter = filter;

            _displayText = new Lazy<string>(GetDisplayText);
        }

        public FileDialogFilter Filter { get; }

        public string DisplayText => _displayText.Value;

        internal static FileDialogFilterViewModel New(FileDialogFilter filter) => new FileDialogFilterViewModel(filter);

        private string GetDisplayText()
        {
            if (Filter.Extensions.Length == 0)
            {
                return Filter.Name;
            }

            var builder = new StringBuilder();

            builder.Append(Filter.Name);
            builder.Append(" (");

            builder.Append("*.");
            builder.Append(Filter.Extensions.First());

            foreach (var extension in Filter.Extensions.Skip(1))
            {
                builder.Append(", *.");
                builder.Append(extension);
            }

            builder.Append(')');

            return builder.ToString();
        }
    }
}
