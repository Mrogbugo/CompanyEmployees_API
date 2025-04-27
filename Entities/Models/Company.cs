using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")] 
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Comapany name is a required field")]
        [MaxLength(60,ErrorMessage = "Maximu length for the Name is 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Comapany name is a required field")]
        [MaxLength(60, ErrorMessage = "Maximu length for the Address  is 60 characters.")] 
        public string? Address  { get; set; }

        public string? Country { get; set; }

      /*  Navigational Properties: can not be mapped as colum 
        these property serves as Relationship between model*/
        public ICollection<Employee>? Employee { get; set; } 
        
    }  

    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Age is a required field.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string? Position { get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        /*  Navigational Properties: can not be mapped as colum 
       these property serves as Relationship between model*/
        public Company? Company { get; set; }

    }
}
