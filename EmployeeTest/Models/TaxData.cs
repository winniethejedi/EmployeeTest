using System.Linq;

namespace EmployeeTest.Models
{
    public static class TaxData
    {
        public const decimal FederalTax = 15;
        public static readonly ILookup<decimal, string> FivePercent = new string[] { "UT", "WY", "NV" }.ToLookup(x => 5M, x => x);
        public static readonly ILookup<decimal, string> SixPointFive = new string[] { "CO", "ID", "AZ", "OR" }.ToLookup(x => 5M, x => x);
        public static readonly ILookup<decimal, string> Seven = new string[] { "WA", "NM", "TX" }.ToLookup(x => 5M, x => x);
    }
}
