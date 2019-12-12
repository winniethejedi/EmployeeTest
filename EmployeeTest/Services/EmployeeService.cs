using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EmployeeTest.Services
{
    public class EmployeeService
    {
        private ParseService ParseService { get; set; }
        private DocumentService DocumentService { get; set; }

        public EmployeeService(ParseService ParseService, DocumentService DocumentService)
        {
            this.ParseService = ParseService;
            this.DocumentService = DocumentService;
        }

        public List<EmployeeModel> GetAllEmployeeData()
        {
            var employeeData = new List<EmployeeModel>();

            string[] lines = DocumentService.GetDocumentLines();

            foreach(string line in lines)
            {
                var employeeModel = GetEmployeeModel(line);
                employeeData.Add(employeeModel);
            } 

            return employeeData;
        }

        private EmployeeModel GetEmployeeModel(string line)
        {
            var employeeRawData = GetRawEmployeeModel(line);
            var employeeModel = GetEmployeeModel(employeeRawData);
            return employeeModel;
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
            var parts = SplitLine(line);

            var rawEmployeeModel = new RawEmployeeModel
            {
                EmployeeId = GetEmployeeId(parts),
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

        private string GetEmployeeId(string[] parts)
        {
            var employeeId = parts[0];
            return employeeId;
        }

        private string[] SplitLine(string line)
        {
            var parts = line.Split(',');
            return parts;
        }

        private string GetEmployeeId(string line)
        {
            var parts = SplitLine(line);
            var employeeId = GetEmployeeId(parts);
            return employeeId;
        }

        public List<TopEarnerModel> GetTopEarners()
        {
            var topEarners = new List<TopEarnerModel>();
            return topEarners;
        }

        public List<EmployeeModel> GetTenRandomEmployeesByEmployeeId(bool logElapsedTime = false)
        {
            var employeeData = GetAllEmployeeData();
            var randomEmployeeIds = GetRandomEmployeeIds(employeeData);

            var returnedEmployeeData = new List<EmployeeModel>();
            Dictionary<string, long> elapsedTimeData = new Dictionary<string, long>();

            foreach (string employeeId in randomEmployeeIds)
            {
                var duplicateEmployee = GetEmployeeByEmployeeId(employeeId, out long elapsedMilliseconds, logElapsedTime);
                returnedEmployeeData.Add(duplicateEmployee);
                elapsedTimeData.Add(employeeId, elapsedMilliseconds);
            }

            DocumentService.CreateElapsedTimeDocument(elapsedTimeData);
            return returnedEmployeeData;
        }

        private List<string> GetRandomEmployeeIds(List<EmployeeModel> employeeData)
        {
            var employeesIdsToGet = new List<string>();
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var randomEmployeeIndex = random.Next(employeeData.Count);
                employeesIdsToGet.Add(employeeData[randomEmployeeIndex].EmployeeId);
            }

            return employeesIdsToGet;
        }

        public EmployeeModel GetEmployeeByEmployeeId(string employeeId, out long elapsedMilliseconds, bool logElapsedTime = false)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var employee = new EmployeeModel();

            string[] lines = DocumentService.GetDocumentLinesWithId(employeeId);

            if (lines.Length == 1)
            {
                employee = GetEmployeeModel(lines[0]);
            }
            else
            {
                foreach (string line in lines)
                {
                    if (employeeId == GetEmployeeId(line))
                    {
                        employee = GetEmployeeModel(line);
                        break;
                    }
                }
            }

            stopwatch.Stop();
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (logElapsedTime)
            {
                Console.WriteLine(elapsedMilliseconds);
            }

            return employee;
        }
    }
}
