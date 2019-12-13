namespace EmployeeTest.Models
{
    public class StateModel
    {
        public string State { get; set; }
        public int MedianTimeWorked { get; set; }
        public decimal MedianNetPay { get; set; }
        public decimal StateTaxes { get; set; }

        public override string ToString()
        {
            string stateModelString = $"{State}, {MedianTimeWorked}, {MedianNetPay},{StateTaxes}";
            return stateModelString;
        }
    }
}
