using System;
using System.Linq;
using System.Reactive.Linq;

using Avalonia;
using Avalonia.Data;

using Movere.Models;

namespace Movere
{
    internal sealed class LocalizedStringExtension
    {
        private sealed class LocalizedStringBinding : IBinding
        {
            public LocalizedStringBinding(object key, object?[] args)
            {
                Key = key;
                Args = args;
            }

            public object Key { get; }

            public object?[] Args { get; }

            public InstancedBinding? Initiate(
                IAvaloniaObject target,
                AvaloniaProperty? targetProperty,
                object? anchor = null,
                bool enableDataValidation = false)
            {
                var observableKey = Key is IBinding binding
                    ? binding.Initiate(target, null)?.Observable
                    : Observable.Return<object?>(Key);

                var observableArgsList =
                    from x in Args
                    select x is IBinding binding
                        ? binding.Initiate(target, null)?.Observable
                        : Observable.Return<object?>(x);

                var observableArgs = observableArgsList.Any()
                    ? Observable.CombineLatest(observableArgsList)
                    : Observable.Return(Array.Empty<object?>());

                return InstancedBinding.OneWay(
                    from key in observableKey
                    from args in observableArgs
                    select String.Format((key as LocalizedString)?.GetString() ?? String.Empty, args)
                );
            }
        }

        public LocalizedStringExtension(object key)
        {
            Key = key;
            Args = Array.Empty<object?>();
        }

        public LocalizedStringExtension(object key, object? arg0)
        {
            Key = key;
            Args = new object?[] { arg0 };
        }

        public LocalizedStringExtension(object key, object? arg0, object? arg1)
        {
            Key = key;
            Args = new object?[] { arg0, arg1 };
        }

        public LocalizedStringExtension(object key, object? arg0, object? arg1, object? arg2)
        {
            Key = key;
            Args = new object?[] { arg0, arg1, arg2 };
        }

        public object Key { get; }

        public object?[] Args { get; }

        public IBinding ProvideValue(IServiceProvider serviceProvider) =>
            new LocalizedStringBinding(Key, Args);
    }
}
