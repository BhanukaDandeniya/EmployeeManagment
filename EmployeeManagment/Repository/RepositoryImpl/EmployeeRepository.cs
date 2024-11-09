using EmployeeManagment.Domain;
using EmployeeManagment.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace EmployeeManagment.Repository.RepositoryImpl
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new MySqlCommand("SELECT * FROM employee", connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32("id"),
                        EmpNo = reader.GetString("empNo"),
                        EmpName = reader.GetString("empName"),
                        EmpAddressLine1 = reader.GetString("empAddressLine1"),
                        EmpAddressLine2 = reader.IsDBNull(reader.GetOrdinal("empAddressLine2")) ? null : reader.GetString("empAddressLine2"),
                        EmpAddressLine3 = reader.IsDBNull(reader.GetOrdinal("empAddressLine3")) ? null : reader.GetString("empAddressLine3"),
                        EmpDateOfJoin = reader.GetDateTime("empDateOfJoin"),
                        EmpStatus = reader.GetBoolean("empStatus"),
                        EmpImage = reader.GetString("empImage")
                    });
                }
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("SELECT * FROM employee WHERE id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    Id = reader.GetInt32("id"),
                    EmpNo = reader.GetString("empNo"),
                    EmpName = reader.GetString("empName"),
                    EmpAddressLine1 = reader.GetString("empAddressLine1"),
                    EmpAddressLine2 = reader.IsDBNull(reader.GetOrdinal("empAddressLine2")) ? null : reader.GetString("empAddressLine2"),
                    EmpAddressLine3 = reader.IsDBNull(reader.GetOrdinal("empAddressLine3")) ? null : reader.GetString("empAddressLine3"),
                    EmpDateOfJoin = reader.GetDateTime("empDateOfJoin"),
                    EmpStatus = reader.GetBoolean("empStatus"),
                    EmpImage = reader.GetString("empImage")
                };
            }
            return null;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand(@"
            INSERT INTO employee (empNo, empName, empAddressLine1, empAddressLine2, 
            empAddressLine3, empDateOfJoin, empStatus, empImage)
            VALUES (@EmpNo, @EmpName, @EmpAddressLine1, @EmpAddressLine2,
            @EmpAddressLine3, @EmpDateOfJoin, @EmpStatus, @EmpImage);
            SELECT LAST_INSERT_ID();", connection);

            command.Parameters.AddWithValue("@EmpNo", employee.EmpNo);
            command.Parameters.AddWithValue("@EmpName", employee.EmpName);
            command.Parameters.AddWithValue("@EmpAddressLine1", employee.EmpAddressLine1);
            command.Parameters.AddWithValue("@EmpAddressLine2", employee.EmpAddressLine2 ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmpAddressLine3", employee.EmpAddressLine3 ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmpDateOfJoin", employee.EmpDateOfJoin);
            command.Parameters.AddWithValue("@EmpStatus", employee.EmpStatus);
            command.Parameters.AddWithValue("@EmpImage", employee.EmpImage);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        // Method for PUT - Complete update
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // First check if employee exists
            using (var checkCommand = new MySqlCommand("SELECT COUNT(1) FROM employee WHERE id = @Id", connection))
            {
                checkCommand.Parameters.AddWithValue("@Id", employee.Id);
                var exists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Employee with ID {employee.Id} not found.");
                }
            }

            // Proceed with complete update
            using var command = new MySqlCommand(@"
                UPDATE employee 
                SET empNo = @EmpNo, 
                    empName = @EmpName,
                    empAddressLine1 = @EmpAddressLine1,
                    empAddressLine2 = @EmpAddressLine2,
                    empAddressLine3 = @EmpAddressLine3,
                    empDateOfJoin = @EmpDateOfJoin,
                    empStatus = @EmpStatus,
                    empImage = @EmpImage
                WHERE id = @Id", connection);

            command.Parameters.AddWithValue("@Id", employee.Id);
            command.Parameters.AddWithValue("@EmpNo", employee.EmpNo);
            command.Parameters.AddWithValue("@EmpName", employee.EmpName);
            command.Parameters.AddWithValue("@EmpAddressLine1", employee.EmpAddressLine1);
            command.Parameters.AddWithValue("@EmpAddressLine2", employee.EmpAddressLine2 ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmpAddressLine3", employee.EmpAddressLine3 ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmpDateOfJoin", employee.EmpDateOfJoin);
            command.Parameters.AddWithValue("@EmpStatus", employee.EmpStatus);
            command.Parameters.AddWithValue("@EmpImage", employee.EmpImage);

            await command.ExecuteNonQueryAsync();
        }

        // Method for PATCH - Partial update
        public async Task PatchEmployeeAsync(int id, EmployeePatchDTO updates)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // Check if the employee exists
            using (var checkCommand = new MySqlCommand("SELECT COUNT(1) FROM employee WHERE id = @Id", connection))
            {
                checkCommand.Parameters.AddWithValue("@Id", id);
                var exists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }
            }

            // Construct update statements only for properties that are provided in the DTO (not null)
            var setClauses = new List<string>();
            var command = new MySqlCommand("", connection);

            if (updates.EmpNo != null)
            {
                setClauses.Add("EmpNo = @EmpNo");
                command.Parameters.AddWithValue("@EmpNo", updates.EmpNo);
            }
            if (updates.EmpName != null)
            {
                setClauses.Add("EmpName = @EmpName");
                command.Parameters.AddWithValue("@EmpName", updates.EmpName);
            }
            if (updates.EmpAddressLine1 != null)
            {
                setClauses.Add("EmpAddressLine1 = @EmpAddressLine1");
                command.Parameters.AddWithValue("@EmpAddressLine1", updates.EmpAddressLine1);
            }
            if (updates.EmpAddressLine2 != null)
            {
                setClauses.Add("EmpAddressLine2 = @EmpAddressLine2");
                command.Parameters.AddWithValue("@EmpAddressLine2", updates.EmpAddressLine2 ?? (object)DBNull.Value);
            }
            if (updates.EmpAddressLine3 != null)
            {
                setClauses.Add("EmpAddressLine3 = @EmpAddressLine3");
                command.Parameters.AddWithValue("@EmpAddressLine3", updates.EmpAddressLine3 ?? (object)DBNull.Value);
            }
            if (updates.EmpDateOfJoin.HasValue)
            {
                setClauses.Add("EmpDateOfJoin = @EmpDateOfJoin");
                command.Parameters.AddWithValue("@EmpDateOfJoin", updates.EmpDateOfJoin.Value);
            }
            if (updates.EmpStatus.HasValue)
            {
                setClauses.Add("EmpStatus = @EmpStatus");
                command.Parameters.AddWithValue("@EmpStatus", updates.EmpStatus.Value);
            }
            if (updates.EmpImage != null)
            {
                setClauses.Add("EmpImage = @EmpImage");
                command.Parameters.AddWithValue("@EmpImage", updates.EmpImage);
            }

            // If no fields are provided in the DTO, exit
            if (!setClauses.Any()) return;

            // Finalize the SQL command for updating
            command.CommandText = $@"
        UPDATE employee 
        SET {string.Join(", ", setClauses)}
        WHERE id = @Id";

            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("DELETE FROM employee WHERE id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}
