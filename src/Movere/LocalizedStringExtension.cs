using System;
using System.Diagnostics.CodeAnalysis;

using Movere.Models;
using Movere.Resources;

namespace Movere
{
    internal sealed class LocalizedStringExtension
    {
        public LocalizedStringExtension(string key)
        {
            Key = key;
        }

        public string Key { get; }

        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Markup extension contract")]
        public string ProvideValue(IServiceProvider serviceProvider) =>
            new LocalizedString(Strings.ResourceManager, Key).GetString();
    }
}
