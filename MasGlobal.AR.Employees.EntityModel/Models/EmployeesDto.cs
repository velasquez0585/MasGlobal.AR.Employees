using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace MasGlobal.AR.Employees.EntityModel.Models
{
    public class EmployeesDto : EmployeesOrigin
    {
        //calculated
        [Display(Name = "Annual salary")]
        public decimal? annualSalary { get; set; }


    }
}
