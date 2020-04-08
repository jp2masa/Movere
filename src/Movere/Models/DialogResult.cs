using System.Diagnostics.CodeAnalysis;

using Movere.Resources;

namespace Movere.Models
{
    public sealed class DialogResult : IDialogResult
    {
        public static IDialogResult OK { get; } = new DialogResult(LocalizedName(nameof(Strings.OK)));
        public static IDialogResult Cancel { get; } = new DialogResult(LocalizedName(nameof(Strings.Cancel)));

        public static IDialogResult Yes { get; } = new DialogResult(LocalizedName(nameof(Strings.Yes)));
        public static IDialogResult No { get; } = new DialogResult(LocalizedName(nameof(Strings.No)));

        public static IDialogResult Abort { get; } = new DialogResult(LocalizedName(nameof(Strings.Abort)));
        public static IDialogResult Retry { get; } = new DialogResult(LocalizedName(nameof(Strings.Retry)));
        public static IDialogResult Ignore { get; } = new DialogResult(LocalizedName(nameof(Strings.Ignore)));

        private readonly LocalizedString _name;

        public DialogResult(LocalizedString name)
        {
            _name = name;
        }

        [SuppressMessage("Globalization", "CA1304:Specify CultureInfo")]
        public string Name => _name.GetString();

        private static LocalizedString LocalizedName(string resourceName) =>
            new LocalizedString(Strings.ResourceManager, resourceName);
    }
}
