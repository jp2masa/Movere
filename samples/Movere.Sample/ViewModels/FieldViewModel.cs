using System;

using ReactiveUI;

namespace Movere.Sample.ViewModels
{
    internal sealed class FieldViewModel : ReactiveObject
    {
        public FieldViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string Value
        {
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        } = String.Empty;
    }
}
