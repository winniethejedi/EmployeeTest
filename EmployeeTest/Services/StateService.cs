using EmployeeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeTest.Services
{
    public class StateService
    {
        private PaycheckService PaycheckService { get; set; }
        private DocumentService DocumentService { get; set; }
        private MedianService MedianService { get; set; }

        public StateService(PaycheckService PaycheckService, DocumentService DocumentService, MedianService MedianService)
        {
            this.PaycheckService = PaycheckService;
            this.DocumentService = DocumentService;
            this.MedianService = MedianService;
        }

        public List<StateModel> GetStateData()
        {
            var paycheckData = PaycheckService.GetPaychecks();
            return GetStateData(paycheckData);
        }

        public List<StateModel> GetStateData(List<PaycheckModel> paychecks)
        {
            var statesData = new List<StateModel>();
            var states = paychecks.Select(x => x.HomeState).Distinct().ToList();

            foreach (var state in states)
            {
                var stateData = paychecks.Where(x => x.HomeState == state).ToList();

                var stateModel = new StateModel
                {
                    MedianNetPay = GetMedianNetPay(stateData),
                    MedianTimeWorked = GetMedianTimeWorked(stateData),
                    State = state,
                    StateTaxes = stateData.Sum(x => x.StateTax)
                };

                statesData.Add(stateModel);
            }

            statesData = statesData.OrderBy(x => x.State).ToList();
            DocumentService.CreateStateDocument(statesData);
            return statesData;
        }

        private int GetMedianTimeWorked(List<PaycheckModel> stateData)
        {
            var hoursWorked = stateData.Select(x => x.HoursWorked).ToList();
            int medianHoursWorked = MedianService.GetMedian(hoursWorked);
            return medianHoursWorked;
        }

        private decimal GetMedianNetPay(List<PaycheckModel> stateData)
        {
            var netPay = stateData.Select(x => x.NetPay).ToList();
            decimal medianNetPay = MedianService.GetMedian(netPay);
            medianNetPay = Math.Round(medianNetPay, 2);
            return medianNetPay;
        }
    }
}
