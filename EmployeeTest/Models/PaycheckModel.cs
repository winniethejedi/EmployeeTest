﻿using System;

namespace EmployeeTest.Models
{
    public class PaycheckModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal GrossPay { get; set; }
        public decimal FederalTax { get; set; }
        public decimal StateTax { get; set; }
        public decimal NetPay { get; set; }
        public DateTime StartDate { get; set; }
        public string HomeState { get; set; }
        public int HoursWorked { get; set; }

        public override string ToString()
        {
            string payCheckModelString = $"{EmployeeId},{FirstName},{LastName},{GrossPay},{FederalTax},{StateTax},{NetPay}";
            return payCheckModelString;
        }
    }
}
