using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HR_Management.Models
{
    public partial class Project
    {
        public Project()
        {
            Employees = new HashSet<Employee>();
        }


        [Key]
        public int Project_ID { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(100, ErrorMessage = "Project Name cannot be longer than 100 characters.")]
        public string Project_Name { get; set; }

        [Required(ErrorMessage ="Start Date is required.")]
        public DateTime Start_Date { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime End_Date { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(50, ErrorMessage = "Status cannot be longer than 50 characters.")]
        public string Status { get; set; }

        [MaxLength(200, ErrorMessage = "Notes cannot be longer than 200 characters.")]
        public string Notes { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
