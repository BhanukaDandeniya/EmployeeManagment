using EmployeeManagment.Domain;
using EmployeeManagment.DTOs;

namespace EmployeeManagment.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(EmployeeDTO employeeDto);
        Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDto);

        Task PatchEmployeeAsync(int id, EmployeePatchDTO updates);
        Task DeleteEmployeeAsync(int id);
    }
}
