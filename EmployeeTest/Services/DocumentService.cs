using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmployeeTest.Services
{
    public class DocumentService
    {
        public const string FileDirectory = @"C:\Users\Owner\source\repos\EmployeeTest\Documents\";
        public const string FileLocation = FileDirectory + @"Employees.txt";

        //Ensures that the directory exists.
        public DocumentService()
        {
            Directory.CreateDirectory(FileDirectory);
        }

        public string[] GetDocumentLines()
        {
            string[] lines = File.ReadAllLines(FileLocation);
            return lines;
        }

        public string[] GetDocumentLinesWithId(string employeeId)
        {
            string[] lines = GetDocumentLines();

            //TODO: Add explanatory comment.
            string employeeIdPlusComma = employeeId + ",";

            string[] idLines = lines.Where(x => x.Contains(employeeIdPlusComma)).ToArray();
            return idLines;
        }

        internal void CreatePaychecksDocument(List<PaycheckModel> paychecks)
        {
            List<string> lines = new List<string>();

            foreach (var paycheck in paychecks)
            {
                lines.Add(paycheck.ToString());
            }

            string fileName = "paychecks_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            string fileLocation = FileDirectory + fileName;
            File.WriteAllLines(fileLocation, lines);
        }
    }
}
