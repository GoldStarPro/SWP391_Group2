using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Month
    {
        public Month()
        {
            SalaryStatistics = new HashSet<SalaryStatistic>();
        }

        public int Month_ID { get; set; }
        public string Month_Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<SalaryStatistic> SalaryStatistics { get; set; }
    }
}
