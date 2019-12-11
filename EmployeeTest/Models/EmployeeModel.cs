using System;

namespace EmployeeTest.Models
{
    public class EmployeeModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PayType PayType { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public string HomeState { get; set; }
        public int HoursWorked { get; set; }
    }

    public class RawEmployeeModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayType { get; set; }
        public string Salary { get; set; }
        public string StartDate { get; set; }
        public string HomeState { get; set; }
        public string HoursWorked { get; set; }
    }
}
