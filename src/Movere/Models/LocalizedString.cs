using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;

namespace Movere.Models
{
    public sealed class LocalizedString
    {
        private readonly ResourceManager _resourceManager;
        private readonly string _resourceName;

        public LocalizedString(ResourceManager resourceManager, string resourceName)
        {
            _resourceManager = resourceManager;
            _resourceName = resourceName;
        }

        [SuppressMessage("Globalization", "CA1304:Specify CultureInfo")]
        public string GetString() => _resourceManager.GetString(_resourceName);

        public string GetString(CultureInfo culture) => _resourceManager.GetString(_resourceName, culture);
    }
}
