using System;

using ReactiveUI;

namespace Movere.Sample.ViewModels
{
    internal class FieldViewModel : ReactiveObject
    {
        private string _value = String.Empty;

        public FieldViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }
}
