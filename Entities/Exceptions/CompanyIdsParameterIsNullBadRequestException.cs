using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
public sealed class CompanyIdsParameterIsNullBadRequestException: BadRequestException
    {
        public CompanyIdsParameterIsNullBadRequestException() : base ("Company Ids collection sent from client is null.")
            {
            }
    }
}
