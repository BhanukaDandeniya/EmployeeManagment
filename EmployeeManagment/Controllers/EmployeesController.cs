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
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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

        // PATCH: api/Employee/{id} // in there you should change the request body according to what are the fields you want to update

        //example if you want to update only employee Name only send the employeeName with request body
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
