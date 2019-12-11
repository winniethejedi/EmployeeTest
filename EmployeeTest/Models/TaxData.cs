using System.Linq;

namespace EmployeeTest.Models
{
    public static class TaxData
    {
        public const decimal FederalTax = 0.15M;
        public const decimal FivePercent = 0.05M;
        public const decimal SixPointFivePercent = 0.065M;
        public const decimal SevenPercent = 0.07M;

        public static ILookup<decimal, string> StateTaxes
        {
            get
            {
                string[] fivePercentStates = new string[] { "UT", "WY", "NV" };
                string[] sixPointFivePercentStates = new string[] { "CO", "ID", "AZ", "OR" };
                string[] sevenPercentStates = new string[] { "WA", "NM", "TX" };
                string[] states = fivePercentStates.Concat(sixPointFivePercentStates).Concat(sevenPercentStates).ToArray();

                var stateTaxes = states.ToLookup(x => {
                    if (fivePercentStates.Contains(x))
                    {
                        return 0.05M;
                    }
                    else if (sixPointFivePercentStates.Contains(x))
                    {
                        return 0.065M;
                    }
                    else if (sevenPercentStates.Contains(x))
                    {
                        return 0.07M;
                    }
                    else
                    {
                        return 0M;
                    }
                }, x => x);

                return stateTaxes;
            }
        }
    }
}
