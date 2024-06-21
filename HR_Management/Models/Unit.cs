using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Unit
    {
        public Unit()
        {
            Salarys = new HashSet<Salary>();
            Employees = new HashSet<Employee>();
        }

        public int Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
