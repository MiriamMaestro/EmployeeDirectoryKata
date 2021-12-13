using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDirectory.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public  string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }
        public  string Department { get; set; }

    }
}
