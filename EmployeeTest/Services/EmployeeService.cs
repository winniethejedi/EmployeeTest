using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EmployeeTest
{
    public class EmployeeService
    {
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
                HoursWorked = ParseInt(employeeRawData.HoursWorked, "Hours Worked"),
                LastName = employeeRawData.LastName,
                PayType = GetPayType(employeeRawData.PayType),
                Salary = ParseDecimal(employeeRawData.Salary, "Salary"),
                StartDate = ParseStartDate(employeeRawData.StartDate)
            };

            return employeeModel;
        }

        private DateTime ParseStartDate(string startDateString)
        {
            bool isParseSuccessful = DateTime.TryParseExact(startDateString, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

            if (!isParseSuccessful)
            {
                ThrowParseException(startDateString, "Start Date");
            }

            return startDate;
        }

        private decimal ParseDecimal(string decimalString, string decimalName)
        {
            bool isParseSuccessful = decimal.TryParse(decimalString, out decimal parsedDecimal);

            if (!isParseSuccessful)
            {
                ThrowParseException(decimalString, decimalName);
            }

            return parsedDecimal;
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

        private void ThrowParseException(string value, string name)
        {
            string message = string.Format("Failed to parse the following {0}: {1}.", value, name);
            throw new FormatException(message);
        }

        private int ParseInt(string intString, string intName)
        {
            bool isParseSuccessful = int.TryParse(intString, out int parsedInt);

            if (!isParseSuccessful)
            {
                ThrowParseException(intString, intName);
            }

            return parsedInt;
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
