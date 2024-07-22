using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace HR_Management.Models
{
    public partial class Employee
    {
        public Employee()
        {
            SalaryStatistics = new HashSet<SalaryStatistic>();
        }

        [Key]
        public int Employee_ID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(50, ErrorMessage = "Full Name cannot be longer than 50 characters.")]
        public string Full_Name { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required.")]
        public DateTime Date_Of_Birth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [MaxLength(6, ErrorMessage = "Gender cannot be longer than 6 characters.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "ID Card Number is required.")]
        [MaxLength(12, ErrorMessage = "ID Card Number cannot be longer than 12 characters.")]
        public string ID_Card_Number { get; set; }

        [Required(ErrorMessage = "Place Of Birth is required.")]
        [MaxLength(100, ErrorMessage = "Place Of Birth cannot be longer than 100 characters.")]
        public string Place_Of_Birth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [MaxLength(12, ErrorMessage = "Phone Number cannot be longer than 12 characters.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }

        public int Qualification_ID { get; set; }

        public int Social_Insurance_ID { get; set; }

        public int Salary_ID { get; set; }

        public int Unit_ID { get; set; }

        public int Project_ID { get; set; }

        public int Tax_ID { get; set; }

        public int Expertise_ID { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(30, ErrorMessage = "Password cannot be longer than 30 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Permission is required.")]
        public int Permission { get; set; }

        public string Image { get; set; }

        [MaxLength(100, ErrorMessage = "Notes cannot be longer than 100 characters.")]
        public string Notes { get; set; }


        [MaxLength(30, ErrorMessage = "Ethnicity cannot be longer than 30 characters.")]
        public string Ethnicity { get; set; }


        [MaxLength(30, ErrorMessage = "Religion cannot be longer than 30 characters.")]
        public string Religion { get; set; }


        [MaxLength(30, ErrorMessage = "Nationality cannot be longer than 30 characters.")]
        public string Nationality { get; set; }

        public virtual SocialInsurance SocialInsuranceIDNavigation { get; set; }
        public virtual Expertise ExpertiseIDNavigation { get; set; }
        public virtual Unit UnitIDNavigation { get; set; }
        public virtual Project ProjectIDNavigation { get; set; }
        public virtual Salary SalaryIDNavigation { get; set; }
        public virtual Qualification QualificationIDNavigation { get; set; }
        public virtual PersonalIncomeTax TaxIDNavigation { get; set; }
        public virtual ICollection<SalaryStatistic> SalaryStatistics { get; set; }

        [NotMapped]
        public IFormFile ConvertImage { get; set; }
    }
}
