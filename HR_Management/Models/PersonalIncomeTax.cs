using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class PersonalIncomeTax
    {
        public PersonalIncomeTax()
        {
            Employees = new HashSet<Employee>();
        }

        public int Tax_ID { get; set; }
        public string Tax_Authority { get; set; }
        public int? Salary_ID { get; set; }
        public int? Amount { get; set; }
        public DateTime? Registration_Date { get; set; }
        public string Notes { get; set; }

        public virtual Salary SalaryIDNavigation { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
