using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.Text;

namespace CompanyEmployees
{
    public class CSVOutputFormatter : TextOutputFormatter
    {
        public CSVOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(CompanyDto).IsAssignableFrom(type) || typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type) ||
                typeof(EmployeeDto).IsAssignableFrom(type) || typeof(IEnumerable<EmployeeDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<CompanyDto> || context.Object is CompanyDto)
            {
                if (context.Object is IEnumerable<CompanyDto>)
                {
                    foreach (var company in (IEnumerable<CompanyDto>)context.Object)
                    {
                        FormatCsv(buffer, company);
                    }
                }
                else
                {
                    FormatCsv(buffer, (CompanyDto)context.Object);

                }

            }

            if (context.Object is IEnumerable<EmployeeDto> || context.Object is EmployeeDto)
            {
                if (context.Object is IEnumerable<EmployeeDto>)
                {
                    foreach (var employee in (IEnumerable<EmployeeDto>)context.Object)
                    {
                        FormatCsv(buffer, employee);
                    }
                }
                else
                {
                    FormatCsv(buffer, (EmployeeDto)context.Object);
                }
            }
            await response.WriteAsync(buffer.ToString());
        }

        private static void FormatCsv(StringBuilder buffer, CompanyDto company)
        {
            buffer.AppendLine($"{company.Id},\" {company.Name},\"{company.FullAddress}\"");
        }

        private static void FormatCsv(StringBuilder buffer, EmployeeDto employee)
        {
            buffer.AppendLine($"{employee.Id},\" {employee.Name} ,\" {employee.Age}, {employee.Position}\"");
        }

       
    }

}
