using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class AddressSegmentChildViewModel : ReactiveObject
    {
        private readonly Folder _folder;

        public AddressSegmentChildViewModel(AddressSegmentViewModel owner, Folder folder)
        {
            Owner = owner;

            _folder = folder;
        }

        public AddressSegmentViewModel Owner { get; }

        public string Name =>
            _folder.Name;

        public string Address =>
            _folder.FullPath;
    }
}
