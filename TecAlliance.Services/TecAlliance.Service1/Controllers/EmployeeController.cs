using Microsoft.AspNetCore.Mvc;
using Serilog;
using TecAlliance.Service1.Models;
using TecAlliance.Service1.Repositories;

namespace TecAlliance.Service1.Controllers
{
    [Route("api/employees")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllEmployees();
            Log.Information("Retrieved all employees.");

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            Log.Information($"Retrieved employee {employee.Name}.");

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data.");
            }

            await _employeeRepository.AddEmployee(employee);
            Log.Information($"Created employee with ID {employee.Id}.");

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data or ID mismatch.");
            }

            await _employeeRepository.UpdateEmployee(employee);
            Log.Information($"Updated employee with ID {employee.Id}.");

            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployee(id);
            Log.Information($"Deleted employee with ID {id}.");

            return NoContent();
        }
    }
}
