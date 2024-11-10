# Employee Management System

A comprehensive employee management system built with ASP.NET Core MVC using ADO.NET for data access. This application provides functionality for managing employee records including CRUD operations (Create, Read, Update, Delete).

## 🚀 Features

- **Employee Management**
  - View all employees in a paginated list
  - Add new employees with detailed information
  - Edit existing employee records
  - Delete employee records
  - Search and filter capabilities

- **Data Persistence**
  - Utilizes ADO.NET for direct database operations
  - Efficient SQL query execution
  - Secure data handling

## 🛠️ Technologies Used

- ASP.NET Core API
- ADO.NET
- SQL Server
- Bootstrap 5
- jQuery
- JavaScript
- HTML5/CSS3

## 📋 Prerequisites

- Visual Studio 2022 (or later)
- .NET 6.0 SDK or later
- SQL Server 2019 (or later)
- SQL Server Management Studio (SSMS)

## ⚙️ Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/BhanukaDandeniya/EmployeeManagment.git
   cd EmployeeManagment
   ```

2. **Database Setup**
   - Open SQL Server Management Studio
   - Create a new database named `EmployeeDB`
   - Execute the following SQL script to create the required table:

   ```sql
   CREATE TABLE Employee (
       EmployeeId INT PRIMARY KEY IDENTITY(1,1),
       Name VARCHAR(100),
       Age INT,
       Email VARCHAR(100),
       Department VARCHAR(50),
       Salary DECIMAL(18,2)
   )
   ```

3. **Update Connection String**
   - Open `appsettings.json`
   - Update the connection string to match your SQL Server configuration:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

4. **Build and Run**
   - Open the solution in Visual Studio
   - Build the solution (Ctrl + Shift + B)
   - Press F5 to run the application

## 📁 Project Structure

```
EmployeeManagement/
├── Controllers/
│   └── EmployeeController.cs
├── Models/
│   └── Employee.cs
├── Views/
│   └── Employee/
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Delete.cshtml
├── Data/
│   └── DataAccess.cs
└── wwwroot/
    ├── css/
    └── js/
```

## 💻 Usage

1. **View Employees**
   - Navigate to the homepage to see the list of all employees
   - Use the search box to filter employees
   - Click on column headers to sort the data

2. **Add New Employee**
   - Click on "Add New Employee" button
   - Fill in the required information
   - Click "Save" to create new employee record

3. **Edit Employee**
   - Click on the "Edit" button next to an employee
   - Modify the information as needed
   - Click "Save" to update the record

4. **Delete Employee**
   - Click on the "Delete" button next to an employee
   - Confirm the deletion in the popup dialog

## ⚠️ Common Issues and Troubleshooting

1. **Database Connection Issues**
   - Verify SQL Server is running
   - Check connection string in appsettings.json
   - Ensure proper permissions are set

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
