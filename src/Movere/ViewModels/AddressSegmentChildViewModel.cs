using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    public sealed class AddressSegmentChildViewModel : ReactiveObject
    {
        private readonly Folder _folder;

        public AddressSegmentChildViewModel(Folder folder)
        {
            _folder = folder;
        }

        public string Name =>
            _folder.Name;

        public string Address =>
            _folder.FullPath;
    }
}
