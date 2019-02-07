using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLoginApp.Models
{
    public class MainViewModels
    {
        public Employee employee { get; set; }
        public LoginUser Login { get; set; }
        public List<Employee> emplist { get; set; }
        public List<LoginUser> loginlist { get; set; }
    }
}
