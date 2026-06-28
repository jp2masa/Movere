using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

using DynamicData;

using ReactiveUI;

using Movere.Models;
using Movere.Reactive;

namespace Movere.ViewModels
{
    public sealed class FileExplorerAddressBarViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<ReadOnlyObservableCollection<AddressSegmentViewModel>> _addressSegments;

        public FileExplorerAddressBarViewModel()
        {
            NavigateToAddressCommand = ReactiveCommand.Create<string>(NavigateToAddress);
            EnterEditModeCommand = ReactiveCommand.Create(() => { IsEditing = true; });

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
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        }

        public string Address
        {
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        } = String.Empty;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TextBoxAddress
        {
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        } = String.Empty;

        public IObservable<string> AddressChanged { get; }

        public ReadOnlyObservableCollection<AddressSegmentViewModel> AddressSegments => _addressSegments.Value;

        public ReactiveCommand<string, Unit> NavigateToAddressCommand { get; }

        public ReactiveCommand<Unit, Unit> EnterEditModeCommand { get; }

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

        private IEnumerable<AddressSegmentViewModel> GetAddressSegments(string address)
        {
            if (Directory.Exists(address))
            {
                var path = Path.GetFullPath(address);
                var directory = new DirectoryInfo(path);

                return GetAddressSegments(new Folder(directory));
            }

            return [];
        }

        private IEnumerable<AddressSegmentViewModel> GetAddressSegments(Folder folder)
        {
            if (folder.Parent is { } parent)
            {
                foreach (var segment in GetAddressSegments(parent))
                {
                    yield return segment;
                }
            }

            yield return new AddressSegmentViewModel(this, folder);
        }
    }
}
