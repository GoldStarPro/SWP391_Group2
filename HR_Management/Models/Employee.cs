using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace HR_Management.Models
{
    public partial class Employee
    {
        public Employee()
        {
            //SalaryStatistics = new HashSet<SalaryStatistic>();
        }

        public int Employee_ID { get; set; }
        public string Full_Name { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public string Gender { get; set; }
        public string ID_Card_Number { get; set; }
        public string Place_Of_Birth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? Qualification_ID { get; set; }
        public int? Social_Insurance_ID { get; set; }
        public int? Salary_ID { get; set; }
        public int? Unit_ID { get; set; }
        public int? Project_ID { get; set; }
        public int? Tax_ID { get; set; }
        public int? Expertise_ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Permisson { get; set; }
        public string Image { get; set; }
        public string Notes { get; set; }
        public string Ethnicity { get; set; }
        public string Religion { get; set; }
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
