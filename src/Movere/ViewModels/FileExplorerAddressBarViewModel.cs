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
    internal sealed class FileExplorerAddressBarViewModel : ReactiveObject, IDisposable
    {
        private readonly Subject<AddressPiece> _addressPieceOpened;

        private string _address;
        private ObservableCollection<AddressPiece> _addressPieces;

        public FileExplorerAddressBarViewModel()
        {
            _address = String.Empty;
            _addressPieces = new ObservableCollection<AddressPiece>();

            AddressPieces = new ReadOnlyObservableCollection<AddressPiece>(_addressPieces);

            _addressPieceOpened = new Subject<AddressPiece>();
            AddressPieceOpened = _addressPieceOpened.AsObservable();

            OpenAddressPieceCommand = ReactiveCommand.Create<AddressPiece>(_addressPieceOpened.OnNext);

            AddressChanged = this.WhenAnyValue(vm => vm.Address);
            AddressChanged.Subscribe(UpdateAddressPieces);
        }

        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        public IObservable<string> AddressChanged { get; }

        public ReadOnlyObservableCollection<AddressPiece> AddressPieces { get; }

        public ICommand OpenAddressPieceCommand { get; }

        public IObservable<AddressPiece> AddressPieceOpened { get; }

        private void UpdateAddressPieces(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var directory = new DirectoryInfo(path);

                _addressPieces.Clear();
                AddAddressPieces(_addressPieces, directory);
            }
        }

        private void AddAddressPieces(ObservableCollection<AddressPiece> pieces, DirectoryInfo directory)
        {
            var parent = directory.Parent;

            if (parent != null)
            {
                AddAddressPieces(pieces, parent);
            }

            pieces.Add(new AddressPiece(directory.Name, directory));
        }

        public void Dispose() => _addressPieceOpened.Dispose();
    }
}
