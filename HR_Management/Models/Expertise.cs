using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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


        [Key]
        public int Expertise_ID { get; set; }

        [Required(ErrorMessage = "Expertise Name is required.")]
        [MaxLength(50, ErrorMessage = "Expertise Name cannot be longer than 50 characters.")]
        public string Expertise_Name { get; set; }

        [MaxLength(100, ErrorMessage = "Notes cannot be more than 100 characters.")]
        public string Notes { get; set; }

        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}