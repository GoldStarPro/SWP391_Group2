using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HR_Management.Models
{
    public partial class PersonalIncomeTax
    {
        public PersonalIncomeTax()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int Tax_ID { get; set; }

        [Required(ErrorMessage = "Tax Authority is required.")]
        [MaxLength(100, ErrorMessage = "Tax Authority cannot be longer than 100 characters.")]
        public string Tax_Authority { get; set; }

        public int Salary_ID { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Registration Date is required.")]
        public DateTime Registration_Date { get; set; }

        [MaxLength(50, ErrorMessage = "Notes cannot be longer than 50 characters.")]
        public string Notes { get; set; }

        public virtual Salary SalaryIDNavigation { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
