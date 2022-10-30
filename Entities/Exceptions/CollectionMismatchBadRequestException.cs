using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CollectionMismatchBadRequestException : BadRequestException
    {
        public CollectionMismatchBadRequestException() : base("The number of company Ids matched with DB entries are different.")
        {

        }
    }
}
