using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Employee name is a required feild.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is a required feild.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required feild.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the position is 20. characters")]
        public string? Position { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
    }
}
