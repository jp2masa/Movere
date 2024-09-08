using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;

using DynamicData;

using ReactiveUI;

using Movere.Models;
using Movere.Reactive;

namespace Movere.ViewModels
{
    public sealed class FileExplorerAddressBarViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<ReadOnlyObservableCollection<AddressSegmentViewModel>> _addressSegments;

        private bool _isEditing;

        private string _address = String.Empty;
        private string _textBoxAddress = String.Empty;

        public FileExplorerAddressBarViewModel()
        {
            NavigateToAddressCommand = ReactiveCommand.Create<string>(NavigateToAddress);

            AddressChanged = this.WhenAnyValue(vm => vm.Address);
            AddressChanged.Subscribe(x => TextBoxAddress = x);

            _addressSegments = (
                from address in AddressChanged
                select GetAddressSegments(address)
                    .ToObservable()
                    .ToObservableChangeSet()
                    .SubscribeRoc()
            )
                .ToProperty(this, x => x.AddressSegments);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => this.RaiseAndSetIfChanged(ref _isEditing, value);
        }

        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TextBoxAddress
        {
            get => _textBoxAddress;
            set => this.RaiseAndSetIfChanged(ref _textBoxAddress, value);
        }

        public IObservable<string> AddressChanged { get; }

        public ReadOnlyObservableCollection<AddressSegmentViewModel> AddressSegments => _addressSegments.Value;

        public ICommand NavigateToAddressCommand { get; }

        public void CancelNavigation()
        {
            TextBoxAddress = Address;
            IsEditing = false;
        }

        public void CommitNavigation()
        {
            Address = TextBoxAddress;
            IsEditing = false;
        }

        private void NavigateToAddress(string address) => Address = address;

        private static IEnumerable<AddressSegmentViewModel> GetAddressSegments(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var directory = new DirectoryInfo(path);

                return GetAddressSegments(new Folder(directory));
            }

            return [];
        }

        private static IEnumerable<AddressSegmentViewModel> GetAddressSegments(Folder folder)
        {
            if (folder.Parent is { } parent)
            {
                foreach (var segment in GetAddressSegments(parent))
                {
                    yield return segment;
                }
            }

            yield return new AddressSegmentViewModel(folder);
        }
    }
}
