using System.Diagnostics.CodeAnalysis;

using Movere.Resources;

namespace Movere.Models
{
    public sealed class DialogResult
    {
        public static DialogResult OK { get; } = FromResource(nameof(Strings.OK));
        public static DialogResult Cancel { get; } = FromResource(nameof(Strings.Cancel));

        public static DialogResult Yes { get; } = FromResource(nameof(Strings.Yes));
        public static DialogResult No { get; } = FromResource(nameof(Strings.No));

        public static DialogResult Abort { get; } = FromResource(nameof(Strings.Abort));
        public static DialogResult Retry { get; } = FromResource(nameof(Strings.Retry));
        public static DialogResult Ignore { get; } = FromResource(nameof(Strings.Ignore));

        private readonly LocalizedString _name;

        public DialogResult(LocalizedString name)
        {
            _name = name;
        }

        [SuppressMessage("Globalization", "CA1304:Specify CultureInfo")]
        public string Name => _name.GetString();

        private static DialogResult FromResource(string name) =>
            new DialogResult(LocalizedName(name));

        private static LocalizedString LocalizedName(string resourceName) =>
            new LocalizedString(Strings.ResourceManager, resourceName);
    }
}
