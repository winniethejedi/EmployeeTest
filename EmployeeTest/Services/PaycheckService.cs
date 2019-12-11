using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EmployeeTest.Services
{
    public class PaycheckService
    {
        private EmployeeService EmployeeService { get; set; }

        public PaycheckService(EmployeeService EmployeeService)
        {
            this.EmployeeService = EmployeeService;
        }

        public List<PaycheckModel> GetPaychecks()
        {
            var paychecks = new List<PaycheckModel>();
            var employeeDatas = EmployeeService.GetAllEmployeeData();

            foreach (var employeeData in employeeDatas)
            {
                PaycheckModel paycheckModel = GetPaycheckModel(employeeData);
                paychecks.Add(paycheckModel);
            }

            return paychecks;
        }

        private PaycheckModel GetPaycheckModel(EmployeeModel employeeData)
        {
            var paycheckModel = new PaycheckModel
            {
                EmployeeId = employeeData.EmployeeId,
                FirstName = employeeData.FirstName,
                LastName = employeeData.LastName,
                GrossPay = GetGrossPay(employeeData),
                FederalTax = TaxData.FederalTax,
                //StateTax = GetStateTax(employeeData.HomeState)
            };

            paycheckModel.NetPay = GetNetPay(paycheckModel);
            return paycheckModel;
        }

        private decimal GetNetPay(PaycheckModel paycheckModel)
        {
            decimal netPay = paycheckModel.GrossPay;
            netPay -= paycheckModel.FederalTax * paycheckModel.GrossPay;
            netPay -= paycheckModel.StateTax * paycheckModel.GrossPay;
            return netPay;
        }

        //private decimal GetStateTax(string homeState)
        //{
        //    decimal stateTax = 0;

        //    stateTax = TaxData.StateTaxes.
        //}

        private decimal GetGrossPay(EmployeeModel employeeData)
        {
            throw new NotImplementedException();
        }
    }
}
