using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;

namespace Movere.Models
{
    public sealed class LocalizedString
    {
        private readonly ResourceManager? _resourceManager;
        private readonly string _resourceName;

        public LocalizedString(ResourceManager resourceManager, string resourceName)
        {
            _resourceManager = resourceManager;
            _resourceName = resourceName;
        }

        public LocalizedString(string value)
        {
            _resourceName = value;
        }

        [SuppressMessage("Globalization", "CA1304:Specify CultureInfo")]
        public string GetString() =>
            _resourceManager?.GetString(_resourceName) ?? _resourceName;

        public string GetString(CultureInfo culture) =>
            _resourceManager?.GetString(_resourceName, culture) ?? _resourceName;

        public static implicit operator LocalizedString(string value) =>
            new LocalizedString(value);
    }
}
