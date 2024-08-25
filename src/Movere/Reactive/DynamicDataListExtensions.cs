using System;
using System.Collections.ObjectModel;

using DynamicData;

namespace Movere.Reactive
{
    internal static class DynamicDataListExtensions
    {
        public static ReadOnlyObservableCollection<T> SubscribeRoc<T>(this IObservable<IChangeSet<T>> source)
            where T : notnull
        {
            source
                .Bind(out var items)
                .Subscribe();

            return items;
        }
    }
}
