using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmployeeTest.Services
{
    public class EmployeeService
    {
        private ParseService ParseService { get; set; }

        public EmployeeService(ParseService ParseService)
        {
            this.ParseService = ParseService;
        }

        public List<EmployeeModel> GetAllEmployeeData()
        {
            var employeeData = new List<EmployeeModel>();

            string[] lines = File.ReadAllLines(@"E:\Users\Owner\Downloads\AaronTemp\Employees\Employees.txt");

            foreach(string line in lines)
            {
                var employeeRawData = GetRawEmployeeModel(line);
                var employeeModel = GetEmployeeModel(employeeRawData);
            } 

            return employeeData;
        }

        private EmployeeModel GetEmployeeModel(RawEmployeeModel employeeRawData)
        {
            var employeeModel = new EmployeeModel
            {
                EmployeeId = employeeRawData.EmployeeId,
                FirstName = employeeRawData.FirstName,
                HomeState = employeeRawData.HomeState,
                HoursWorked = ParseService.ParseInt(employeeRawData.HoursWorked, "Hours Worked"),
                LastName = employeeRawData.LastName,
                PayType = GetPayType(employeeRawData.PayType),
                Salary = ParseService.ParseDecimal(employeeRawData.Salary, "Salary"),
                StartDate = ParseService.ParseStartDate(employeeRawData.StartDate)
            };

            return employeeModel;
        }

        private PayType GetPayType(string payTypeString)
        {
            var payType = PayType.H;

            if (payTypeString == PayType.S.ToString())
            {
                payType = PayType.S;
            }

            return payType;
        }

        private RawEmployeeModel GetRawEmployeeModel(string line)
        {
            var parts = line.Split(',');

            var rawEmployeeModel = new RawEmployeeModel
            {
                EmployeeId = parts[0],
                FirstName = parts[1],
                LastName = parts[2],
                PayType = parts[3],
                Salary = parts[4],
                StartDate = parts[5],
                HomeState = parts[6],
                HoursWorked = parts[7]
            };

            return rawEmployeeModel;
        }

        public List<TopEarnerModel> GetTopEarners()
        {
            var topEarners = new List<TopEarnerModel>();
            return topEarners;
        }

        public EmployeeModel GetEmployeeByEmployeeId()
        {
            throw new NotImplementedException();
        }
    }
}
