using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Qualification
    {
        public Qualification()
        {
            Salarys = new HashSet<Salary>();
            Employees = new HashSet<Employee>();
        }

        public int Qualification_ID { get; set; }
        public string Qualification_Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
