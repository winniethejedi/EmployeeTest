using System.ComponentModel;

namespace EmployeeTest.Models
{
    public enum PayType
    {
        [Description("Hourly")]
        H,
        [Description("Salaried")]
        S
    }
}
