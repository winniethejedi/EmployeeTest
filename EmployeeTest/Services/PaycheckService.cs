using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeTest.Services
{
    public class PaycheckService
    {
        private EmployeeService EmployeeService { get; set; }
        private DocumentService DocumentService { get; set; }

        public PaycheckService(EmployeeService EmployeeService, DocumentService DocumentService)
        {
            this.EmployeeService = EmployeeService;
            this.DocumentService = DocumentService;
        }

        public List<PaycheckModel> GetPaychecks()
        {
            var employeeDatas = EmployeeService.GetAllEmployeeData();
            return GetPaychecks(employeeDatas);
        }

        public List<PaycheckModel> GetPaychecks(List<EmployeeModel> employeeDatas)
        {
            var paychecks = new List<PaycheckModel>();

            foreach (var employeeData in employeeDatas)
            {
                PaycheckModel paycheckModel = GetPaycheckModel(employeeData);
                paychecks.Add(paycheckModel);
            }

            paychecks = paychecks.OrderByDescending(x => x.GrossPay).ToList();
            DocumentService.CreatePaychecksDocument(paychecks);
            return paychecks;
        }

        public List<TopEarnerModel> GetTopEarners()
        {
            var paychecks = GetPaychecks();
            return GetTopEarners(paychecks);
        }

        public List<TopEarnerModel> GetTopEarners(List<PaycheckModel> paychecks)
        {
            var topEarnersPaychecks = GetTopEarnersPaychecks(paychecks);
            var topEarners = new List<TopEarnerModel>();

            foreach (var paycheck in topEarnersPaychecks)
            {
                TopEarnerModel topEarner = GetTopEarner(paycheck);
                topEarners.Add(topEarner);
            }

            topEarners = topEarners.OrderByDescending(x => x.YearsWorked).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
            DocumentService.CreateTopEarnersDocument(topEarners);
            return topEarners;
        }

        private TopEarnerModel GetTopEarner(PaycheckModel paycheck)
        {
            var topEarner = new TopEarnerModel
            {
                FirstName = paycheck.FirstName,
                GrossPay = paycheck.GrossPay,
                LastName = paycheck.LastName,
                YearsWorked = GetYearsWorked(paycheck.StartDate)
            };

            return topEarner;
        }

        private int GetYearsWorked(DateTime startDate)
        {
            int yearsWorked = DateTime.Now.Year - startDate.Year;
            return yearsWorked;
        }

        private List<PaycheckModel> GetTopEarnersPaychecks(List<PaycheckModel> paychecks)
        {
            int take = (int)Math.Ceiling(paychecks.Count * 0.15);
            var topEarnersPaychecks = paychecks.Take(take).ToList();
            return topEarnersPaychecks;
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
                StateTax = GetStateTax(employeeData.HomeState),
                StartDate = employeeData.StartDate,
                HomeState = employeeData.HomeState,
                HoursWorked = employeeData.HoursWorked
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
