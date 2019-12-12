using System.Collections.Generic;
using System.Linq;

namespace EmployeeTest.Models
{
    public static class TaxData
    {
        public const decimal FederalTax = 0.15M;
        public const decimal FivePercent = 0.05M;
        public const decimal SixPointFivePercent = 0.065M;
        public const decimal SevenPercent = 0.07M;
        public static readonly Dictionary<string, decimal> StateTaxes = new Dictionary<string, decimal>
        {
            { "UT", FivePercent},
            { "WY", FivePercent},
            { "NV", FivePercent},
            { "CO", SixPointFivePercent},
            { "ID", SixPointFivePercent},
            { "AZ", SixPointFivePercent},
            { "OR", SixPointFivePercent},
            { "WA", SevenPercent},
            { "NM", SevenPercent},
            { "TX", SevenPercent},
        };
    }
}
