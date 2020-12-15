using Microsoft.VisualStudio.TestTools.UnitTesting;
using MasGlobal.AR.Employees.EntityModel.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasGlobal.AR.Employees.BusinessLogic.Test
{

    [TestClass]
    public class EmployeeListTest
    {
        //not to repeat the configuration
        public IConfiguration _myConfiguration()
        {
            //Configuration
            var _myConfiguration = new Dictionary<string, string>
                {
                    {"valores:ApiExternal", "http://masglobaltestapi.azurewebsites.net/api/Employees"}
                };
            var _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(_myConfiguration)
                .Build();
            return _configuration;
        }


        [TestMethod]
        public async Task VerifyThatTheEmployeeListIsNull()
        {

            var _employeesLogic = new Employees.EmployeesBusinessLogic(_myConfiguration());
            List<EmployeesDto> _result = await _employeesLogic.GetAllEmployees();
            Assert.IsNull( _result);

        }

        [TestMethod]
        public async Task VerifyThatTheEmployeeListIsNotNull()
        {

            var _employeesLogic = new Employees.EmployeesBusinessLogic(_myConfiguration());
            List<EmployeesDto> _result = await _employeesLogic.GetAllEmployees();
            Assert.IsNotNull(_result);

        }

    }
}
