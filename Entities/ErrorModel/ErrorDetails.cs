using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            //JsonSerializer : Provides functionality to serialize objects or value types to JSON
            //and to deserialize JSON into objects or value types.
            return JsonSerializer.Serialize(this);
        }
    }
}
