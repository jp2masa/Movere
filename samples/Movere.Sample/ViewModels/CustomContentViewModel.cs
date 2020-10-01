using System.Collections.Generic;

namespace Movere.Sample.ViewModels
{
    internal sealed class CustomContentViewModel
    {
        public CustomContentViewModel(IReadOnlyList<FieldViewModel> fields)
        {
            Fields = fields;
        }

        public IReadOnlyList<FieldViewModel> Fields { get; }
    }
}
