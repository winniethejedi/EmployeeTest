using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmployeeTest.Services
{
    public class DocumentService
    {
        //This should be changed to whichever folder on your computer you want to save the documents
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

            //This should help further narrow the results.
            string employeeIdPlusComma = employeeId + ",";

            string[] idLines = lines.Where(x => x.Contains(employeeIdPlusComma)).ToArray();
            return idLines;
        }

        internal void CreatePaychecksDocument(List<PaycheckModel> paychecks)
        {
            var lines = paychecks.Select(x => x.ToString()).ToArray();
            CreateFile(lines, "paychecks_");
        }

        internal void CreateElapsedTimeDocument(Dictionary<string, long> elapsedTimeData)
        {
            var lines = elapsedTimeData.Select(x => x.ToString()).ToArray();
            CreateFile(lines, "elapsed_time_");
        }

        internal void CreateStateDocument(List<StateModel> statesData)
        {
            var lines = statesData.Select(x => x.ToString()).ToArray();
            CreateFile(lines, "states_");
        }

        internal void CreateTopEarnersDocument(List<TopEarnerModel> topEarners)
        {
            var lines = topEarners.Select(x => x.ToString()).ToArray();
            CreateFile(lines, "top_earners_");
        }

        private string CreateFileName(string prefix)
        {
            string fileName = prefix + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            return fileName;
        }

        private void CreateFile(string[] lines, string prefix)
        {
            string fileName = CreateFileName(prefix);
            string fileLocation = FileDirectory + fileName;
            File.WriteAllLines(fileLocation, lines);
        }
    }
}
