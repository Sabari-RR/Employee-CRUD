using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLoginApp.Models
{
    [Table("LoginUser")]
    public class LoginUser
    {
        [Key]
        public int UserID { get; set; }
        public DateTime Logintime { get; set; }
        public DateTime Logouttime { get; set; }
        public String Status { get; set; }
        public double TotalHours { get; set; }
    }
}
