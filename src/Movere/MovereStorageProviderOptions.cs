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
    }
}
