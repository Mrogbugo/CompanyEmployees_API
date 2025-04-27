using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Share.DataTransferObjects
{
    public record CompanyForCreationDto : CompanyForManipulationDto;
    //(string Name, string Address, string Country,IEnumerable<EmployeeForCreationDto> Employees); 


}
