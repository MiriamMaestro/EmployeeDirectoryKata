using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Repositories
{
    public interface IRepository
    {
        IEnumerable<Employee> GetEmployees();
        IEnumerable<Employee> GetAll();
        Task<Employee> GetEmployeeAsync(int id);
        Employee AddEmployee(Employee employee);
        void DeleteEmployee(int id);
        void UpdateEmployee(Employee employee);
    }
}
