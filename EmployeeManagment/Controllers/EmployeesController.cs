using EmployeeManagment.Domain;
using EmployeeManagment.DTOs;
using EmployeeManagment.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(EmployeeDTO employeeDto)
        {
            var id = await _employeeService.CreateEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { id = id }, employeeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(id, employeeDto);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PATCH: api/Employee/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, [FromBody] EmployeePatchDTO updates)
        {
            if (updates == null) return BadRequest("Invalid data.");

            try
            {
                await _employeeService.PatchEmployeeAsync(id, updates);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
