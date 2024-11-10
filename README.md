# Employee Management System
A comprehensive employee management system built with ASP.NET Core Web API using ADO.NET for MySQL data access. This application provides REST API endpoints for managing employee records including CRUD operations.

## 🚀 Features
- **Employee Management**
  - Get all employees 
  - Get employee by ID
  - Add new employee
  - Update employee details
  - Delete employee
  

- **Data Persistence**
  - Utilizes ADO.NET for direct MySQL database operations
  - Efficient SQL query execution
  - Secure data handling

## 🛠️ Technologies Used
- ASP.NET Core Web API
- ADO.NET
- MySQL
- Swagger UI for API documentation

## 📋 Prerequisites
- Visual Studio 2022 (or later)
- .NET 6.0 SDK or later
- MySQL Server
- MySQL Workbench

## ⚙️ Setup Instructions
1. **Clone the Repository**
   ```bash
   git clone https://github.com/BhanukaDandeniya/EmployeeManagment.git
   cd EmployeeManagment
   ```

2. **Database Setup**
   - Open MySQL Workbench
   - Create a new database named `employee_db`
   - Execute the following SQL script:
   ```sql
   CREATE TABLE Employee (
       EmployeeId INT PRIMARY KEY AUTO_INCREMENT,
       Name VARCHAR(100),
       Age INT,
       Email VARCHAR(100),
       Department VARCHAR(50),
       Salary DECIMAL(18,2)
   );
   ```

3. **Update Connection String**
   - Open `appsettings.json`
   - Update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=employee_db;Uid=root;Pwd=your_password;"
   }
   ```

4. **Build and Run**
   - Open the solution in Visual Studio
   - Restore NuGet packages
   - Build the solution (Ctrl + Shift + B)
   - Press F5 to run the application

## 📁 Project Structure
```
EmployeeManagement/
├── Controllers/
│   └── EmployeeController.cs
├── Models/
│   └── Employee.cs
├── Data/
│   └── DataAccess.cs
└── Properties/
    └── launchSettings.json
```

## 💻 API Endpoints
1. **GET /api/employees**
   - Retrieves all employees
   - Optional query parameters for search and filter

2. **GET /api/employees/{id}**
   - Retrieves specific employee by ID

3. **POST /api/employees**
   - Creates new employee
   - Request body: Employee object

4. **PUT /api/employees/{id}**
   - Updates existing employee
   - Request body: Employee object

5. **DELETE /api/employees/{id}**
   - Deletes employee by ID

## ⚠️ Common Issues and Troubleshooting
1. **Database Connection Issues**
   - Verify MySQL Server is running
   - Check connection string in appsettings.json
   - Ensure proper user permissions

2. **Build Errors**
   - Restore NuGet packages
   - Clean and rebuild solution
   - Verify .NET SDK version

## 🤝 Contributing
1. Fork the repository
2. Create a new branch
3. Make your changes
4. Submit a pull request

## 📝 License
This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors
- Bhanuka Dandeniya

## 📧 Support
For support and queries, please create an issue in the repository.
