using System;
using System.Globalization;

namespace EmployeeTest.Services
{
    public class ParseService
    {
        private void ThrowParseException(string value, string name)
        {
            string message = string.Format("Failed to parse the following {0}: {1}.", value, name);
            throw new FormatException(message);
        }

        public int ParseInt(string intString, string intName)
        {
            bool isParseSuccessful = int.TryParse(intString, out int parsedInt);

            if (!isParseSuccessful)
            {
                ThrowParseException(intString, intName);
            }

            return parsedInt;
        }

        public DateTime ParseStartDate(string startDateString)
        {
            bool isParseSuccessful = DateTime.TryParseExact(startDateString, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

            if (!isParseSuccessful)
            {
                ThrowParseException(startDateString, "Start Date");
            }

            return startDate;
        }

        public decimal ParseDecimal(string decimalString, string decimalName)
        {
            bool isParseSuccessful = decimal.TryParse(decimalString, out decimal parsedDecimal);

            if (!isParseSuccessful)
            {
                ThrowParseException(decimalString, decimalName);
            }

            return parsedDecimal;
        }
    }
}
