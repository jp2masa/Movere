using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public class AddressSegmentViewModel : ReactiveObject
    {
        private static readonly Func<Folder, AddressSegmentViewModel> s_createAddressSegment = x => new AddressSegmentViewModel(x);

        private readonly Folder _folder;

        private bool _isPopupOpen;

        public AddressSegmentViewModel(Folder folder)
        {
            _folder = folder;

            OpenPopupCommand = ReactiveCommand.Create(() => IsPopupOpen = true);
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => this.RaiseAndSetIfChanged(ref _isPopupOpen, value);
        }

        public string Name => _folder.Name;

        public string Address => _folder.FullPath;

        public IEnumerable<AddressSegmentViewModel> Children => _folder.Folders.Select(s_createAddressSegment);

        public ICommand OpenPopupCommand { get; }
    }
}
