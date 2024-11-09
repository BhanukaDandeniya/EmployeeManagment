namespace EmployeeManagment.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string EmpAddressLine1 { get; set; }
        public string? EmpAddressLine2 { get; set; }
        public string? EmpAddressLine3 { get; set; }
        public DateTime EmpDateOfJoin { get; set; }
        public bool EmpStatus { get; set; }
        public string EmpImage { get; set; }
    }
}
