using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Expertise
    {
        public Expertise()
        {
            Salarys = new HashSet<Salary>();
            Employees = new HashSet<Employee>();
        }

        public int Expertise_ID { get; set; }
        public string Expertise_Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
