using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeDirectory.Data;
using EmployeeDirectory.Models;
using EmployeeDirectory.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;


namespace EmployeeDirectory.Controllers
{
    [Route("employee")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly ILogger<ValuesController> _logger;
        private readonly EmployeeDBContext _context;

      
        public ValuesController(IRepository repository, ILogger<ValuesController> logger)
        {
            this._repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            var item = _repository.GetEmployees().Select(item => item);
            return item;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeAsync(int id)
        {
            _logger.LogInformation( "Getting item {Id}", id);
            var result = await  _repository.GetEmployeeAsync(id);
            if (result != null) return result;
            _logger.LogWarning("Get({Id}) NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee(Employee employee)
        {
            var result = _repository.AddEmployee(employee);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Employee Created with Name: {employee}", employee.FirstName);
            return result;
        }

        [HttpDelete("{id}")]
        public  IActionResult DeleteEmployee(int id)
        {
            var todoItem = _repository.GetEmployeeAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            _repository.DeleteEmployee(id);
             _logger.LogInformation($"Employee has been deleted");
            return NoContent();

        }

    }
}
