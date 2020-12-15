using System.Collections.Generic;
using MasGlobal.AR.Employees.EntityModel.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace MasGlobal.AR.Employees.AccessLayer.Access
{
    public class EmployeesAccessLayer
    {
        private readonly IConfiguration _configuration;

        public EmployeesAccessLayer(IConfiguration conf)
        {
            _configuration = conf;
        }

        public async Task<List<EmployeesOrigin>> GetAllEmployeesApiExternal()
        {

            string _urlApiExterna = _configuration["valores:ApiExternal"];
            List<EmployeesOrigin> _listEmployeesOrigin = new List<EmployeesOrigin>();
            var _response = new HttpResponseMessage();
            string _stringResult = "";
            using (var client = new HttpClient())
            {

                _response = await client.GetAsync(_urlApiExterna);
                _response.EnsureSuccessStatusCode();
                string _responseBody = await _response.Content.ReadAsStringAsync();
                _stringResult = await _response.Content.ReadAsStringAsync();
                _listEmployeesOrigin = JsonConvert.DeserializeObject<List<EmployeesOrigin>>(_stringResult);

            }

            return _listEmployeesOrigin;
        }

        public async Task<List<EmployeesDto>> GetAllEmployees()
        {
            List<EmployeesOrigin> _listEmployeesOrigin = await GetAllEmployeesApiExternal();

            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>(); ;

            //can be done with mapper.map
            _listEmployeesDto = _listEmployeesOrigin
            .Select(x => new EmployeesDto()
            {
                id  = x.id,
                name = x.name,
                contractTypeName = x.contractTypeName,
                roleId = x.roleId,
                roleName = x.roleName,
                roleDescription = x.roleDescription,
                hourlySalary = x.hourlySalary,
                monthlySalary = x.monthlySalary,                         
            }).ToList();

            return _listEmployeesDto;
        }

        public async Task<List<EmployeesDto>> GetEmployeesForId(int? id)
        {
            //since I always have to find all employees in external api, I always call GetAllEmployees method
            List<EmployeesDto> _listEmployeesDto = await GetAllEmployees();

            //if the id is not null then I limit the search to only the id, but as shown in a grid I always show it as list
            if (id != null)
            {
                _listEmployeesDto = await _listEmployeesDto.Where(emp => emp.id == id).ToAsyncEnumerable().ToList();
            }

            return _listEmployeesDto;
        }




    }
}
