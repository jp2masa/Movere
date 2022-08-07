using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using Avalonia;
using Avalonia.Data;

using Movere.Models;
using Movere.Resources;

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
                    ? (
                        from key in binding.Initiate(target, null)?.Observable
                            ?? Observable.Never<string>().StartWith(String.Empty)
                        select ConvertKey(key)
                    )
                    : Observable.Never<LocalizedString>().StartWith(ConvertKey(Key));

                var observableArgsList =
                    from x in Args
                    select x is IBinding binding
                        ? binding.Initiate(target, null)?.Observable
                        : Observable.Never<object?>().StartWith(x);

                var observableArgs = observableArgsList.Any()
                    ? (
                        from args in Observable.CombineLatest(observableArgsList)
                        select args.ToArray()
                    )
                    : Observable.Never<object?[]>().StartWith(new object?[][] { Array.Empty<object?>() });

                return InstancedBinding.OneWay(
                    from key in observableKey
                    from args in observableArgs
                    select String.Format(key.GetString(), args)
                );
            }

            private static LocalizedString ConvertKey(object? key) =>
                key switch
                {
                    string str => new LocalizedString(Strings.ResourceManager, str),
                    LocalizedString loc => loc,
                    BindingNotification => String.Empty,
                    _ => throw new NotSupportedException("Key must be either string or LocalizedString!")
                };
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
            new LocalizedStringBinding(
                Key is string key
                    ? new LocalizedString(Strings.ResourceManager, key)
                    : Key,
                Args
            );
    }
}
