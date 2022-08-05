using System;

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

        public string ProvideValue(IServiceProvider serviceProvider) =>
            new LocalizedString(Strings.ResourceManager, Key).GetString();
    }
}
