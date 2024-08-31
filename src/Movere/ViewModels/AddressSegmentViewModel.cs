using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class AddressSegmentViewModel : ReactiveObject
    {
        private readonly Folder _folder;

        private bool _isPopupOpen;

        public AddressSegmentViewModel(Folder folder)
        {
            _folder = folder;

            var children = new ObservableCollection<AddressSegmentChildViewModel>(
                _folder.Folders.Select(static x => new AddressSegmentChildViewModel(x))
            );

            Children = new ReadOnlyObservableCollection<AddressSegmentChildViewModel>(children);

            OpenPopupCommand = ReactiveCommand.Create(() => IsPopupOpen = true);
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => this.RaiseAndSetIfChanged(ref _isPopupOpen, value);
        }

        public string Name => _folder.Name;

        public string Address => _folder.FullPath;

        public ReadOnlyObservableCollection<AddressSegmentChildViewModel> Children { get; }

        public ICommand OpenPopupCommand { get; }
    }
}
