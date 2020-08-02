using System;
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
            var builder = new StringBuilder();

            builder.Append(Filter.Name);
            builder.Append(" (");

            var first = true;

            foreach (var extension in Filter.Extensions)
            {
                if (!first)
                {
                    builder.Append(", ");
                    first = false;
                }

                builder.Append("*.");
                builder.Append(extension);
            }

            builder.Append(')');

            return builder.ToString();
        }
    }
}
