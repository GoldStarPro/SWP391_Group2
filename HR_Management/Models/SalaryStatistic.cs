using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class SalaryStatistic
    {
        public int Salary_Statistic_ID { get; set; }
        public int Employee_ID { get; set; }
        public int Month_ID { get; set; }
        public int? BasicSalary { get; set; }
        public int? TaxToPay { get; set; }
        public int? Bonus { get; set; }
        public int? Allowance { get; set; }
        public string Notes { get; set; }
        public int? TotalSalary { get; set; }
        public DateTime? Create_Date { get; set; }

        public virtual Employee EmployeeIDNavigation { get; set; }
        public virtual Month MonthIDNavigation { get; set; }
    }
}
