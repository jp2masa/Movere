namespace Movere
{
    public sealed record MovereStorageProviderOptions
    {
        internal static MovereStorageProviderOptions Default { get; } = new MovereStorageProviderOptions();

        /// <summary>
        /// If the storage provider should be used as a fallback, i.e. the top level platform implementation
        /// should be queried first and, if available, used instead of Movere's implementation
        /// </summary>
        public bool IsFallback { get; init; } = true;

        /// <summary>
        /// If the dialogs should be windows, when possible. Otherwise, they will be overlays.
        /// </summary>
        public bool PreferWindowDialogs { get; init; } = true;
    }
}
