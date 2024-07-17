using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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


        [Key]
        public int Qualification_ID { get; set; }

        [Required(ErrorMessage = "Qualification Name is required.")]
        [MaxLength(50, ErrorMessage = "Qualification Name cannot be longer than 50 characters.")]
        public string Qualification_Name { get; set; }

        [MaxLength(100, ErrorMessage = "Notes cannot be more than 100 characters.")]
        public string Notes { get; set; }

        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
