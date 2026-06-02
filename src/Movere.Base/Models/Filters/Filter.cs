namespace Movere.Models.Filters
{
    internal static class Filter
    {
        private static class Cache<T>
        {
            public static readonly IFilter<T> True = new FuncFilter<T>(static _ => true);

            public static readonly IFilter<T> False = new FuncFilter<T>(static _ => false);
        }

        public static IFilter<T> True<T>() => Cache<T>.True;

        public static IFilter<T> False<T>() => Cache<T>.False;
    }
}
