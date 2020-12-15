using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace MasGlobal.AR.Employees.EntityModel.Models
{
    public class EmployeesOrigin
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Contract type name")]
        public string contractTypeName { get; set; }
        [Display(Name = "Role Id")]
        public int roleId { get; set; }
        [Display(Name = "Role name")]
        public string roleName { get; set; }
        [Display(Name = "Role description")]
        public string roleDescription { get; set; }
        [Display(Name = "Hourly salary")]
        public decimal hourlySalary { get; set; }
        [Display(Name = "Monthly salary")]
        public decimal monthlySalary { get; set; }
    }
}
