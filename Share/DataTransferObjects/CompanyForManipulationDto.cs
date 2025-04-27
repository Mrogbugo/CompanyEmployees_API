using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DataTransferObjects
{
    public record CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Employee Name is a Required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 Characters.")]
        public string? Name { get; init; }
        [Required(ErrorMessage = "Address is a required field")]
        public string Address { get; init; }
        [Required(ErrorMessage = "Country is a required field.")]
        [MaxLength(20, ErrorMessage = "Maxium Length for the Postion is 100")]
        public string Country { get; init; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; init; }
    }
}
