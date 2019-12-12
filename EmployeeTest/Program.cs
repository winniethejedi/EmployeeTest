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

                var employeeData = employeeService.GetAllEmployeeData();

                var paycheckService = new PaycheckService(employeeService);

                var paycheckData = paycheckService.GetPaychecks();

                Console.WriteLine("All done! Good-bye world!");
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(ex);
                Console.WriteLine(json);
            }
        }
    }
}
