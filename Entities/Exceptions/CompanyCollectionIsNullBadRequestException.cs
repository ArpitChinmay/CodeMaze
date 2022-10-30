using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CompanyCollectionIsNullBadRequestException : BadRequestException
    {
        public CompanyCollectionIsNullBadRequestException() : base("The passed companies collection sent from client is null.")
        {
        }
    }
}
