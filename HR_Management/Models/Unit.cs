using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    [Key]
    public int Unit_ID { get; set; }

    [Required(ErrorMessage = "Unit Name is required.")]
    [MaxLength(100, ErrorMessage = "Unit Name cannot be more than 100 characters.")]
    public string Unit_Name { get; set; }

    [MaxLength(100, ErrorMessage = "Notes cannot be more than 100 characters.")]
    public string Notes { get; set; }

    public virtual ICollection<Salary> Salarys { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
  }
}

