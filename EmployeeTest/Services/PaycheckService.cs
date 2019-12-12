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

            paychecks = paychecks.OrderByDescending(x => x.GrossPay).ToList();
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
                StateTax = GetStateTax(employeeData.HomeState)
            };

            paycheckModel.NetPay = GetNetPay(paycheckModel);

            //Round these once we've made the necessary calculations.
            paycheckModel.GrossPay = Math.Round(paycheckModel.GrossPay, 2);
            paycheckModel.FederalTax = Math.Round(paycheckModel.FederalTax, 2);
            paycheckModel.StateTax = Math.Round(paycheckModel.StateTax, 2);
            return paycheckModel;
        }

        private decimal GetNetPay(PaycheckModel paycheckModel)
        {
            decimal netPay = paycheckModel.GrossPay;
            netPay -= paycheckModel.FederalTax * paycheckModel.GrossPay;
            netPay -= paycheckModel.StateTax * paycheckModel.GrossPay;
            netPay = Math.Round(netPay, 2);
            return netPay;
        }

        private decimal GetStateTax(string homeState)
        {
            bool hasState = TaxData.StateTaxes.TryGetValue(homeState, out decimal stateTax);
            
            if (!hasState)
            {
                stateTax = 0;
            }

            return stateTax;
        }

        private decimal GetGrossPay(EmployeeModel employeeData)
        {
            decimal grossPay;

            if (employeeData.PayType == PayType.S)
            {
                grossPay = GetGrossSalaryPay(employeeData.HoursWorked, employeeData.Salary);
            }
            else
            {
                grossPay = GetGrossHourlyPay(employeeData.HoursWorked, employeeData.Salary);
            }

            return grossPay;
        }

        private decimal GetGrossHourlyPay(int hoursWorked, decimal hourlyRate)
        {
            const int maxHours = 80;
            const int maxOvertimeHours = 90;
            decimal grossHourlyPay = 0;

            if (hoursWorked <= maxHours)
            {
                grossHourlyPay = GetPay(hoursWorked, hourlyRate);
            }
            else if (hoursWorked > maxHours && hoursWorked <= maxOvertimeHours)
            {
                decimal normalPay = GetPay(maxHours, hourlyRate);
                int overtimeHours = hoursWorked - maxHours;
                decimal overtimePay = GetOvertimePay(overtimeHours, hourlyRate);
                grossHourlyPay = normalPay + overtimePay;
            }
            //More than 90 hours
            else
            {
                decimal normalHourlyPay = GetPay(maxHours, hourlyRate);
                int overtimeHours = maxOvertimeHours - maxHours;
                decimal overtimeHourlyPay = GetOvertimePay(overtimeHours, hourlyRate);
                int superOvertimeHours = hoursWorked - maxOvertimeHours;
                decimal superOvertimePay = GetSuperOvertimePay(superOvertimeHours, hourlyRate);
            }

            return grossHourlyPay;
        }

        private decimal GetSuperOvertimePay(int superOvertimeHours, decimal hourlyRate)
        {
            decimal rate = GetAdjustedHourlyRate(hourlyRate, 1.75M);
            decimal superOvertimePay = GetPay(superOvertimeHours, rate);
            return superOvertimePay;
        }

        private decimal GetOvertimePay(int overtimeHours, decimal hourlyRate)
        {
            decimal rate = GetAdjustedHourlyRate(hourlyRate, 1.5M);
            decimal overtimeHourlyPay = GetPay(overtimeHours, rate);
            return overtimeHourlyPay;
        }

        private decimal GetAdjustedHourlyRate(decimal originalHourlyRate, decimal adjustment)
        {
            decimal rate = originalHourlyRate * adjustment;
            return rate;
        }

        private decimal GetPay(int hoursWorked, decimal hourlyRate)
        {
            decimal normalHourlyPay = hoursWorked * hourlyRate;
            return normalHourlyPay;
        }

        private decimal GetGrossSalaryPay(int hoursWorked, decimal salary)
        {
            //They get paid every 2 weeks and there's 52 weeks in a year.
            decimal grossSalaryPay = salary / 26;
            return grossSalaryPay;
        }
    }
}
