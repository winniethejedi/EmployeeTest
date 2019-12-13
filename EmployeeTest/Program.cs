using EmployeeTest.Services;
using Newtonsoft.Json;
using System;

namespace EmployeeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //IMPORTANT: Before running this, set the document folder in the DocumentService, and ensure Employees.txt is in that folder.
                Console.WriteLine("Hello World!");
                var parseService = new ParseService();
                var documentService = new DocumentService();
                var employeeService = new EmployeeService(parseService, documentService);

                //1. Gets all paycheck data and creates a document.
                var paycheckService = new PaycheckService(employeeService, documentService);
                var paycheckData = paycheckService.GetPaychecks();

                //2. Gets top earners and creates a document.
                var topEarners = paycheckService.GetTopEarners(paycheckData);

                //3. Gets state data and creates a document
                var medianService = new MedianService();
                var stateService = new StateService(paycheckService, documentService, medianService);
                var stateData = stateService.GetStateData(paycheckData);

                //4. Gets ten random employees by EmployeeId and creates a text file with the time to get each of them.
                var tenRandomEmployees = employeeService.GetTenRandomEmployeesByEmployeeId(logElapsedTime: true);

                Console.WriteLine("All done! Good-bye world!");
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex);
                Console.WriteLine(json);
            }

            Console.Read();
        }
    }
}
