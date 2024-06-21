using System;
using System.Collections.Generic;

#nullable disable

namespace HR_Management.Models
{
    public partial class SocialInsurance
    {
        public SocialInsurance()
        {
            Employees = new HashSet<Employee>();
        }

        public int Social_Insurance_ID { get; set; }
        public DateTime? Issue_Date { get; set; }
        public string Issue_Place { get; set; }
        public string Registered_Medical_Facility { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
