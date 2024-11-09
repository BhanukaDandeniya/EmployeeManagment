using EmployeeManagment.Domain;
using EmployeeManagment.DTOs;
using EmployeeManagment.Repository;
using MySql.Data.MySqlClient;

namespace EmployeeManagment.Service.ServiceImpl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();
                return employees ?? Enumerable.Empty<Employee>();
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all employees");
                throw new ApplicationException("Error occurred while retrieving employees", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching all employees");
                throw new ApplicationException("An unexpected error occurred while retrieving employees", ex);
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found");
                }
                return employee;
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching employee with ID {Id}", id);
                throw new ApplicationException($"Error occurred while retrieving employee with ID {id}", ex);
            }
            catch (KeyNotFoundException)
            {
                throw; // Rethrow KeyNotFoundException as is
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching employee with ID {Id}", id);
                throw new ApplicationException($"An unexpected error occurred while retrieving employee with ID {id}", ex);
            }
        }

        public async Task<int> CreateEmployeeAsync(EmployeeDTO employeeDto)
        {
            try
            {
                ValidateEmployeeDto(employeeDto);

                var employee = new Employee
                {
                    EmpNo = employeeDto.EmpNo,
                    EmpName = employeeDto.EmpName,
                    EmpAddressLine1 = employeeDto.EmpAddressLine1,
                    EmpAddressLine2 = employeeDto.EmpAddressLine2,
                    EmpAddressLine3 = employeeDto.EmpAddressLine3,
                    EmpDateOfJoin = employeeDto.EmpDateOfJoin,
                    EmpStatus = employeeDto.EmpStatus,
                    EmpImage = employeeDto.EmpImage
                };

                return await _employeeRepository.CreateEmployeeAsync(employee);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry error
            {
                _logger.LogError(ex, "Duplicate employee number detected: {EmpNo}", employeeDto.EmpNo);
                throw new InvalidOperationException($"Employee number {employeeDto.EmpNo} already exists", ex);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating employee");
                throw new ApplicationException("Error occurred while creating employee", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating employee");
                throw new ApplicationException("An unexpected error occurred while creating employee", ex);
            }
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDto)
        {
            try
            {
                // Check if employee exists
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found");
                }

                ValidateEmployeeDto(employeeDto);

                var employee = new Employee
                {
                    Id = id,
                    EmpNo = employeeDto.EmpNo,
                    EmpName = employeeDto.EmpName,
                    EmpAddressLine1 = employeeDto.EmpAddressLine1,
                    EmpAddressLine2 = employeeDto.EmpAddressLine2,
                    EmpAddressLine3 = employeeDto.EmpAddressLine3,
                    EmpDateOfJoin = employeeDto.EmpDateOfJoin,
                    EmpStatus = employeeDto.EmpStatus,
                    EmpImage = employeeDto.EmpImage
                };

                await _employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry error
            {
                _logger.LogError(ex, "Duplicate employee number detected during update: {EmpNo}", employeeDto.EmpNo);
                throw new InvalidOperationException($"Employee number {employeeDto.EmpNo} already exists", ex);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating employee with ID {Id}", id);
                throw new ApplicationException($"Error occurred while updating employee with ID {id}", ex);
            }
            catch (KeyNotFoundException)
            {
                throw; // Rethrow KeyNotFoundException as is
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating employee with ID {Id}", id);
                throw new ApplicationException($"An unexpected error occurred while updating employee with ID {id}", ex);
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                // Check if employee exists
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found");
                }

                await _employeeRepository.DeleteEmployeeAsync(id);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting employee with ID {Id}", id);
                throw new ApplicationException($"Error occurred while deleting employee with ID {id}", ex);
            }
            catch (KeyNotFoundException)
            {
                throw; // Rethrow KeyNotFoundException as is
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting employee with ID {Id}", id);
                throw new ApplicationException($"An unexpected error occurred while deleting employee with ID {id}", ex);
            }
        }

        public async Task PatchEmployeeAsync(int id, EmployeePatchDTO updates)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found");
                }

                if (updates != null)
                {
                    await _employeeRepository.PatchEmployeeAsync(id, updates);  // Pass DTO directly
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error while patching employee with ID {Id}", id);
                throw new ApplicationException($"Error updating employee with ID {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while patching employee with ID {Id}", id);
                throw new ApplicationException($"Unexpected error updating employee with ID {id}", ex);
            }
        }

        private void ValidateEmployeeDto(EmployeeDTO employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto));

            if (string.IsNullOrWhiteSpace(employeeDto.EmpNo))
                throw new ArgumentException("Employee number is required", nameof(employeeDto));

            if (string.IsNullOrWhiteSpace(employeeDto.EmpName))
                throw new ArgumentException("Employee name is required", nameof(employeeDto));

            if (string.IsNullOrWhiteSpace(employeeDto.EmpAddressLine1))
                throw new ArgumentException("Employee address line 1 is required", nameof(employeeDto));

            if (employeeDto.EmpDateOfJoin == default)
                throw new ArgumentException("Employee date of join is required", nameof(employeeDto));

            if (string.IsNullOrWhiteSpace(employeeDto.EmpImage))
                throw new ArgumentException("Employee image is required", nameof(employeeDto));
        }
    }
}
