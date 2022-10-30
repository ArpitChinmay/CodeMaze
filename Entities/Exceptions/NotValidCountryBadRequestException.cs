using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class NotValidCountryBadRequestException : BadRequestException
    {
        public NotValidCountryBadRequestException() : base("The country passed in route is not a valid country.")
        { }
    }
}
