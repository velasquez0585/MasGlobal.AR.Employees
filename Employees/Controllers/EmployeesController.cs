using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MasGlobal.AR.Employees.EntityModel.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace Employees.Controllers
{   
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _configuration;
        public EmployeesController(IConfiguration conf)
        {
            _configuration = conf;
        }

        public virtual async Task<IActionResult> Index()
        {

            string urlApiExterna = _configuration["valores:Api"];
            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            var response = new HttpResponseMessage();
            string stringResult = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApiExterna);
                response = await client.GetAsync($"api/Employees/GetAllEmployees/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stringResult = await response.Content.ReadAsStringAsync();
                _listEmployeesDto = JsonConvert.DeserializeObject<List<EmployeesDto>>(stringResult);

            }

            return View(_listEmployeesDto);

        }

        public async Task<IActionResult> searchEmployees(int? id)
        {
            string urlApiExterna = _configuration["valores:ApiExternal"];
            List<EmployeesDto> _listEmployeesDto = new List<EmployeesDto>();
            var response = new HttpResponseMessage();
            string stringResult = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApiExterna);
                response = await client.GetAsync($"api/Employees/GetEmployeesForId/{id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stringResult = await response.Content.ReadAsStringAsync();
                _listEmployeesDto = JsonConvert.DeserializeObject<List<EmployeesDto>>(stringResult);

            }

            return View(_listEmployeesDto);

        }

    }
}