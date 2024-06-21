using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Salary
    {
        public Salary()
        {
            PersonalIncomeTaxs = new HashSet<PersonalIncomeTax>();
            Employees = new HashSet<Employee>();
        }

        public int Salary_ID { get; set; }
        public int? Expertise_ID { get; set; }
        public int? Qualification_ID { get; set; }
        public int? Unit_ID { get; set; }
        public int? Basic_Salary { get; set; }
        public int? New_Basic_Salary { get; set; }
        public DateTime? Entry_Date { get; set; }
        public DateTime? Modify_Date { get; set; }
        public string Notes { get; set; }

        public virtual Expertise ExpertiseIDNavigation { get; set; }
        public virtual Unit UnitIDNavigation { get; set; }
        public virtual Qualification QualificationIDNavigation { get; set; }
        public virtual ICollection<PersonalIncomeTax> PersonalIncomeTaxs { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
