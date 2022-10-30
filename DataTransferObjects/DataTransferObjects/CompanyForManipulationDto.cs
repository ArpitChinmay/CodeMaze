using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is a required feild.")]
        [MaxLength(30, ErrorMessage = "Maximum length for company can be 30 characters.")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Company address is a required feild.")]
        [MaxLength(100, ErrorMessage = "The address can't contain more than 100 characters.")]
        public string Address { get; init; }

        [Required(ErrorMessage = "Country of origin is a required feild.")]
        [MaxLength(20, ErrorMessage = "Length of country can't be more than 20 characters.")]
        [DataType(DataType.Text)]
        public string Country { get; init; }

        [Required(ErrorMessage = "The company needs to have one or more employees.")]
        public IEnumerable<EmployeeForCreationDto> Employees { get; init; }
    }
}
