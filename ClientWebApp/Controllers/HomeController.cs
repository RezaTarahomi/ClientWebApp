using ClientWebApp.Common;
using ClientWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace ClientWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private string apiBaseUrl;

        public HomeController(ILogger<HomeController> logger , IConfiguration Configuration)
        {
            _logger = logger;
            _configuration = Configuration;
            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashbord(int page = 1)
        {            
            return View();
        }       

         

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ErrPartial()
        {
            return PartialView("_ErrPartial");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
