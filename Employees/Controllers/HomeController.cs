using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Employees.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Web.Administration;
using Newtonsoft.Json;

namespace Employees.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserWrapper _userWrapper;
        private readonly IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        //private readonly IServicioNovedades _servNovedades;

        public class ControllerBase : Controller
        {

        }

        public HomeController(UserWrapper userWrapper, IHostingEnvironment env, IConfiguration Configuration, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _userWrapper = userWrapper;
            _environment = env;
            _configuration = Configuration;
            //_servNovedades = sn;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {

            bool simularAutenticacion = Convert.ToBoolean(_configuration["Login:SimularAutenticacion"]);

            if ((_environment.EnvironmentName == "Desa" || _environment.EnvironmentName == "LenovoMMD" || _environment.EnvironmentName == "Test") && Request.Query["user"].Count > 0)
            {
                _userWrapper.UserStub = Request.Query["user"].ToString();

            }


            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

  
            if (!string.IsNullOrEmpty(Request.Query["ReturnUrl"]))
            {
                return Redirect(Request.Query["ReturnUrl"]);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

       
        public IActionResult Index()
        {
            return View("Index");
        }
        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }


        public IActionResult UserInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserInfo(string userName, bool isAdmin, bool reset)
        {
            if (reset)
            {
                _userWrapper.ForceAdmin = false;
                _userWrapper.UserStub = null;
            }
            else
            {
                _userWrapper.ForceAdmin = isAdmin;
                _userWrapper.UserStub = userName;
            }
            return View();
        }

    }
}
