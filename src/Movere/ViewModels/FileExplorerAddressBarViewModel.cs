using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class FileExplorerAddressBarViewModel : ReactiveObject, IDisposable
    {
        private readonly Subject<Folder> _addressPieceOpened = new Subject<Folder>();

        private readonly ObservableCollection<Folder> _addressPieces = new ObservableCollection<Folder>();

        private bool _isEditing;

        private string _address = String.Empty;
        private string _textBoxAddress = String.Empty;

        public FileExplorerAddressBarViewModel()
        {
            AddressPieces = new ReadOnlyObservableCollection<Folder>(_addressPieces);

            AddressPieceOpened = _addressPieceOpened.AsObservable();

            OpenAddressPieceCommand = ReactiveCommand.Create<Folder>(_addressPieceOpened.OnNext);

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

        [Obsolete("This should only be used by the view")]
        public string TextBoxAddress
        {
            get => _textBoxAddress;
            set => this.RaiseAndSetIfChanged(ref _textBoxAddress, value);
        }

        public IObservable<string> AddressChanged { get; }

        public ReadOnlyObservableCollection<Folder> AddressPieces { get; }

        public ICommand OpenAddressPieceCommand { get; }

        public IObservable<Folder> AddressPieceOpened { get; }

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

        private void UpdateAddress(string address)
        {
            TextBoxAddress = address;
            UpdateAddressPieces(address);
        }

        private void UpdateAddressPieces(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var directory = new DirectoryInfo(path);

                _addressPieces.Clear();
                AddAddressPieces(_addressPieces, new Folder(directory));
            }
        }

        private void AddAddressPieces(ObservableCollection<Folder> pieces, Folder folder)
        {
            var parent = folder.Parent;

            if (parent != null)
            {
                AddAddressPieces(pieces, parent);
            }

            pieces.Add(folder);
        }

        public void Dispose() => _addressPieceOpened.Dispose();
    }
}
