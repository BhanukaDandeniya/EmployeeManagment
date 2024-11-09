using EmployeeManagment.Domain;
using EmployeeManagment.DTOs;

namespace EmployeeManagment.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);

        Task PatchEmployeeAsync(int id, EmployeePatchDTO updates);
        Task DeleteEmployeeAsync(int id);
    }
}
