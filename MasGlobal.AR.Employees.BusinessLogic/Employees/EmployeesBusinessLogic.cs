using System.Collections.Generic;
using MasGlobal.AR.Employees.EntityModel.Models;
using MasGlobal.AR.Employees.AccessLayer.Access;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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
            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            EmployeesAccessLayer employeesAccessLayer = new EmployeesAccessLayer(_configuration);
            _listEmployeesDto = await employeesAccessLayer.GetAllEmployees();

            return _listEmployeesDto;
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
