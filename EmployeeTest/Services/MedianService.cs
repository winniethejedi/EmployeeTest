using System.Collections.Generic;
using System.Linq;

namespace EmployeeTest.Services
{
    //Source: https://blogs.msmvps.com/deborahk/linq-mean-median-and-mode/
    public class MedianService
    {
        //TODO: It seems like there should be way to combine these two methods.
        public decimal GetMedian(List<decimal> decimals)
        {
            int numberCount = decimals.Count();
            int halfIndex = numberCount / 2;
            var sortedNumbers = decimals.OrderBy(n => n);
            bool isEven = numberCount % 2 == 0;
            decimal median;

            if (isEven)
            {
                median = (sortedNumbers.ElementAt(halfIndex) +
                    sortedNumbers.ElementAt(halfIndex - 1)) / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        public int GetMedian(List<int> ints)
        {
            int numberCount = ints.Count();
            int halfIndex = numberCount / 2;
            var sortedNumbers = ints.OrderBy(n => n);
            bool isEven = numberCount % 2 == 0;
            int median;

            if (isEven)
            {
                median = (sortedNumbers.ElementAt(halfIndex) +
                    sortedNumbers.ElementAt(halfIndex - 1)) / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }
    }
}
