using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HR_Management.Models
{
    public partial class SocialInsurance
    {
        public SocialInsurance()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int Social_Insurance_ID { get; set; }

        [Required(ErrorMessage = "Issue Date is required.")]
        public DateTime Issue_Date { get; set; }

        [Required(ErrorMessage = "Issue Place is required.")]
        [MaxLength(50, ErrorMessage = "Issue Place cannot be longer than 50 characters.")]
        public string Issue_Place { get; set; }

        [Required(ErrorMessage = "Registered Medical Facility is required.")]
        [MaxLength(100, ErrorMessage = "Registered Medical Facility cannot be longer than 100 characters.")]
        public string Registered_Medical_Facility { get; set; }

        [MaxLength(50, ErrorMessage = "Notes cannot be longer than 50 characters.")]
        public string Notes { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
