using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyDto
    {
        //Init sets the properties or indexers at point of creation and doesn't allow anymore changes afterwards.
        //This allows for much more flexible immutable model in C#.
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }
}
