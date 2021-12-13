using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EmployeeDirectory.Models;
using Microsoft.Data.Sqlite;

namespace EmployeeDirectory.Repositories
{
    public class Repository: IRepository
    {
        public IEnumerable<Employee> GetEmployees()
            => CreateDatabase().Query<Employee>("select * from Employees", new DynamicParameters()).ToList();

        public IEnumerable<Employee> GetAll() 
        => CreateDatabase().Query<Employee>("select * from Employees", new DynamicParameters()).ToList();

        public async Task<Employee> GetEmployeeAsync(int id)
            => await CreateDatabase().QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employees WHERE Id = @id", new { id });

        public Employee AddEmployee(Employee employee)
            => CreateDatabase().ExecuteScalar<Employee>("INSERT INTO Employees (FirstName, LastName, Email, Department) VALUES (@firstName, @lastName, @email, @department)", new {@firstName = employee.FirstName, @lastName= employee.LastName,@email= employee.Email, @department= employee.Department});

        public void DeleteEmployee(int id)
            => CreateDatabase().Execute("DELETE FROM Employees WHERE Id = @id ", new { id });
        public void UpdateEmployee(Employee employee)
        {
            CreateDatabase().Execute(
                "Update Employees Set FirstName = @firstName, LastName = @lastName, Department = @department, Email = @email  where Id=@id",
                new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.Department,
                    employee.Email,
                    employee.EmployeeId
                });
        }
        private SqliteConnection CreateDatabase()
        {
            using var connection = new SqliteConnection("Data Source=EmployeeLibrary.sqlite");
            return connection;
        }
    }
}
