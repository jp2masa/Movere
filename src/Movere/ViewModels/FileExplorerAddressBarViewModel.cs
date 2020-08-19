using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class FileExplorerAddressBarViewModel : ReactiveObject
    {
        private readonly ObservableCollection<AddressSegmentViewModel> _addressSegments = new ObservableCollection<AddressSegmentViewModel>();

        private bool _isEditing;

        private string _address = String.Empty;
        private string _textBoxAddress = String.Empty;

        public FileExplorerAddressBarViewModel()
        {
            AddressSegments = new ReadOnlyObservableCollection<AddressSegmentViewModel>(_addressSegments);

            NavigateToAddressCommand = ReactiveCommand.Create<string>(NavigateToAddress);

            AddressChanged = this.WhenAnyValue(vm => vm.Address);
            AddressChanged.Subscribe(UpdateAddress);
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

        public ReadOnlyObservableCollection<AddressSegmentViewModel> AddressSegments { get; }

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

        private void UpdateAddress(string address)
        {
            TextBoxAddress = address;
            UpdateAddressSegments(address);
        }

        private void UpdateAddressSegments(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var directory = new DirectoryInfo(path);

                _addressSegments.Clear();
                AddAddressSegments(_addressSegments, new Folder(directory));
            }
        }

        private void AddAddressSegments(ObservableCollection<AddressSegmentViewModel> segments, Folder folder)
        {
            var parent = folder.Parent;

            if (parent != null)
            {
                AddAddressSegments(segments, parent);
            }

            segments.Add(new AddressSegmentViewModel(folder));
        }
    }
}
