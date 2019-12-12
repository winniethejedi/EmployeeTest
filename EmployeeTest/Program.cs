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
                Console.WriteLine("Hello World!");
                var parseService = new ParseService();
                var documentService = new DocumentService();
                var employeeService = new EmployeeService(parseService, documentService);

                //1. Creates a document with all paycheck data
                var paycheckService = new PaycheckService(employeeService, documentService);
                var paycheckData = paycheckService.GetPaychecks();

                //TODO: Create overrides since we've already gotten the data?
                //2. Gets top earners
                var topEarners = paycheckService.GetTopEarners();

                //3.

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
