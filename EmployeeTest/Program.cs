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
                var employeeService = new EmployeeService(parseService);

                var employeeData = employeeService.GetAllEmployeeData();

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
