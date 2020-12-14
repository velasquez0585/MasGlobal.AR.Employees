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

        public async Task<List<EmployeesDto>> GetAllEmployees()
        {

            string urlApiExterna = _configuration["valores:ApiExternal"];
            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            var response = new HttpResponseMessage();
            string stringResult = "";
            using (var client = new HttpClient())
            {

                response = await client.GetAsync(urlApiExterna);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stringResult = await response.Content.ReadAsStringAsync();
                _listEmployeesDto = JsonConvert.DeserializeObject<List<EmployeesDto>>(stringResult);

            }

            return  _listEmployeesDto;
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
