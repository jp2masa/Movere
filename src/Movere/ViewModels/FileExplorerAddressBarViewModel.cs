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
        private readonly Subject<Folder> _addressPieceOpened;

        private bool _isEditing;

        private string _address;
        private ObservableCollection<Folder> _addressPieces;

        public FileExplorerAddressBarViewModel()
        {
            EditCommand = ReactiveCommand.Create(() => IsEditing = true);

            _address = String.Empty;
            _addressPieces = new ObservableCollection<Folder>();

            AddressPieces = new ReadOnlyObservableCollection<Folder>(_addressPieces);

            _addressPieceOpened = new Subject<Folder>();
            AddressPieceOpened = _addressPieceOpened.AsObservable();

            OpenAddressPieceCommand = ReactiveCommand.Create<Folder>(_addressPieceOpened.OnNext);

            AddressChanged = this.WhenAnyValue(vm => vm.Address);
            AddressChanged.Subscribe(UpdateAddressPieces);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => this.RaiseAndSetIfChanged(ref _isEditing, value);
        }

        public ICommand EditCommand { get; }

        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        public IObservable<string> AddressChanged { get; }

        public ReadOnlyObservableCollection<Folder> AddressPieces { get; }

        public ICommand OpenAddressPieceCommand { get; }

        public IObservable<Folder> AddressPieceOpened { get; }

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
