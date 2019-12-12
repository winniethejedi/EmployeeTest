using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace EmployeeTest.Services
{
    public class DocumentService
    {
        public const string FileLocation = @"E:\Users\Owner\Downloads\AaronTemp\Employees\Employees.txt";

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
    }
}
