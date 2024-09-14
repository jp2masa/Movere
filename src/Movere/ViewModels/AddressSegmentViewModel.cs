using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class AddressSegmentViewModel : ReactiveObject
    {
        private readonly Folder _folder;

        private bool _isPopupOpen;

        public AddressSegmentViewModel(FileExplorerAddressBarViewModel owner, Folder folder)
        {
            _folder = folder;

            var children = new ObservableCollection<AddressSegmentChildViewModel>(
                _folder.Folders.Select(x => new AddressSegmentChildViewModel(this, x))
            );

            Children = new ReadOnlyObservableCollection<AddressSegmentChildViewModel>(children);

            OpenCommand = ReactiveCommand.CreateFromTask(
                async (string x) =>
                {
                    IsPopupOpen = false;
                    await owner.NavigateToAddressCommand.Execute(x);
                }
            );

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

        public ReactiveCommand<string, Unit> OpenCommand { get; }

        public ICommand OpenPopupCommand { get; }
    }
}
