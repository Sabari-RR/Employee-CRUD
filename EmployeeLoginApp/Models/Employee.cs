using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLoginApp.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Please Provide Employee Name"), MaxLength(20)]
        public String Name { get; set; }
        [Required(ErrorMessage = "Please Provide Employee Email"), EmailAddress]
        public String Email { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please Provide Employee Mobile Number")]
        public Int64 MobileNo { get; set; }
        [Required(ErrorMessage = "Please Provide Employee Role")]
        public String Role { get; set; }
    }
}
