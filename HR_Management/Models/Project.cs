using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class Project
    {
        public Project()
        {
            Employees = new HashSet<Employee>();
        }

        public int Project_ID { get; set; }
        public string Project_Name { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
