using AutoMapper;
using ClientWebApp.Common;
using ClientWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientWebApp.Controllers
{

    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private string apiBaseUrl;

        private readonly IMapper _mapper;

        public AccountController(IConfiguration Configuration, IMapper mapper)
        {
            _configuration = Configuration;
            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel user)
        {
            string loginApiUrl = apiBaseUrl + "api/account/login";
            var response = await GetClient().PostAsync(loginApiUrl, getModelSringContent(user)); 
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                AppendTokensToCookie(contentString);
                return RedirectToAction("Dashbord", "Home");
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("Password", "نام کاربری یا پسورد اشتباه است");
                return View(user);
            }
        }

        private void AppendTokensToCookie(string contentString)
        {
            var tokens = JToken.Parse(contentString);
            var accessToken = tokens[0].ToString();
            var refreshToken = tokens[1].ToString();
            HttpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions { HttpOnly = true, Secure = true });
            HttpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions { HttpOnly = true, Secure = true });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var apiUser = _mapper.Map<RegisterViewModel, ApiUser>(model);
                apiUser.Image = ConvertApp.ToBase64String(model.Image);
                var registerApiUrl = apiBaseUrl + "api/account/register";
                var response = await GetClient().PostAsync(registerApiUrl, getModelSringContent(apiUser));
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return RedirectToAction("UserList", "UserManager");
                }               
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Password",  $" خطا در ثبت کاربر. کد خطا : {response.StatusCode}");
                    return View(model);
                }
            }

            return View(model);

        }

        private HttpContent getModelSringContent<T>(T model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }

        private HttpClient GetClient()
        {
            string access_token = Request.Cookies["access_token"];
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
            return client;
        }        

        public IActionResult LogOut()
        {
            string access_token = Request.Cookies["access_token"];
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> DeleteRefreshToken(string userName)
        { 
            var DeleteRefreshTokenApiUrl = apiBaseUrl + "api/account/logout?userName=" + userName; 
            var response = await GetClient().GetAsync(DeleteRefreshTokenApiUrl);
            return RedirectToAction("Login");
        }

        public async Task<JsonResult> IsLastNameExist(string lastName)
        {
            
            var Url = apiBaseUrl + "api/account/checklastname?lastName=" + lastName;
            var response = await GetClient().GetAsync(Url);
            if (response.StatusCode== HttpStatusCode.Accepted)
            {
                return Json(false);
            }           
            else
            {
                return Json(true);
            }
        }

    }
}
