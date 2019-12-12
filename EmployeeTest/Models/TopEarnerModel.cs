namespace EmployeeTest.Models
{
    public class TopEarnerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearsWorked { get; set; }
        public decimal GrossPay { get; set; }

        public override string ToString()
        {
            string topEarnerModelString = $"{FirstName}, {LastName}, {YearsWorked},{GrossPay}";
            return topEarnerModelString;
        }
    }
}
