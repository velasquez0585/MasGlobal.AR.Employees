using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasGlobal.AR.Employees.BusinessLogic.Employees;
using MasGlobal.AR.Employees.EntityModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MasGlobal.AR.Employees.WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IConfiguration _configuration;
        public EmployeesController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        [HttpGet]
        [Route("GetAllEmployees")]/// Employees/GetAllEmployees
        public async Task<List<EmployeesDto>> GetAllEmployees()
        {
            try
            {
                EmployeesBusinessLogic _employeesBusinessLogic = new EmployeesBusinessLogic(_configuration);

                return await _employeesBusinessLogic.GetEmployeesForId(1);
            }
            catch (Exception ex)
            {
                var errorMessages = "";

                if (ex.InnerException != null)
                    errorMessages += ex.InnerException.Message;

                if (errorMessages == "")
                    if (!string.IsNullOrEmpty(ex.Message))
                        errorMessages = ex.Message;

                if (errorMessages == "")
                    errorMessages = "An error occurred contact systems";

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat("Error Employees->GetAllEmployees: ", errorMessages);

                throw new Exception(exceptionMessage, ex.InnerException);
            }
        }

        [HttpGet]
        [Route("GetEmployeesForId/{id}")]/// Employees/GetAllEmployees
        public async Task<List<EmployeesDto>> GetEmployeesForId(int? id)
        {
            try
            {
                EmployeesBusinessLogic _employeesBusinessLogic = new EmployeesBusinessLogic(_configuration);

                return await _employeesBusinessLogic.GetEmployeesForId(id);
            }
            catch (Exception ex)
            {
                var errorMessages = "";

                if (ex.InnerException != null)
                    errorMessages += ex.InnerException.Message;

                if (errorMessages == "")
                    if (!string.IsNullOrEmpty(ex.Message))
                        errorMessages = ex.Message;

                if (errorMessages == "")
                    errorMessages = "An error occurred contact systems";

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat("Error Employees->GetAllEmployees: ", errorMessages);

                throw new Exception(exceptionMessage, ex.InnerException);
            }
        }

    }
}
