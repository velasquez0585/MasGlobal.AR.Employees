using System.Collections.Generic;
using MasGlobal.AR.Employees.EntityModel.Models;
using MasGlobal.AR.Employees.AccessLayer.Access;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;

namespace MasGlobal.AR.Employees.BusinessLogic.Employees
{
   public class EmployeesBusinessLogic
    {

        private readonly IConfiguration _configuration;

        public EmployeesBusinessLogic(IConfiguration conf)
        {
            _configuration = conf;
        }

        public async Task<List<EmployeesDto>> GetAllEmployees()
        {
            List<EmployeesOrigin> _listEmployeesOrigin = new List<EmployeesOrigin>();
            EmployeesAccessLayer employeesAccessLayer = new EmployeesAccessLayer(_configuration);
            _listEmployeesOrigin = await employeesAccessLayer.GetAllEmployeesApiExternal();


            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            //can be done with mapper.map
            _listEmployeesDto = _listEmployeesOrigin
            .Select(x => new EmployeesDto()
            {
                id = x.id,
                name = x.name,
                contractTypeName = x.contractTypeName,
                roleId = x.roleId,
                roleName = x.roleName,
                roleDescription = x.roleDescription,
                hourlySalary = x.hourlySalary,
                monthlySalary = x.monthlySalary,
                annualSalary = CalculateAnnualSalary(x.contractTypeName, x.hourlySalary, x.monthlySalary)
            }).ToList();


            return _listEmployeesDto;
        }


        public decimal CalculateAnnualSalary(string contractTypeName, decimal hourlySalary, decimal monthlySalary)
        {
            decimal _annualSalary = 0;
            switch (contractTypeName)
            {
                case "HourlySalaryEmployee":
                    _annualSalary = 120 * hourlySalary * 12;
                    break;
                case "MonthlySalaryEmployee":
                    _annualSalary = 12 * monthlySalary;
                    break;
                default:
                    _annualSalary = 0;
                    break;
            }

            return _annualSalary;

        }

        public async Task<List<EmployeesDto>> GetEmployeesForId(int? id)
        {
            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            EmployeesAccessLayer employeesAccessLayer = new EmployeesAccessLayer(_configuration);
            _listEmployeesDto = await employeesAccessLayer.GetEmployeesForId(id);

            return _listEmployeesDto;
        }


    }
}
