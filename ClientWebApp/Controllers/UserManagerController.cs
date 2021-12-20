

using AutoMapper;
using ClientWebApp.Common;
using ClientWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientWebApp.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IConfiguration _configuration;
        private string apiBaseUrl;
        private readonly IMapper _mapper;
        public UserManagerController(IConfiguration Configuration, IMapper mapper)
        {
            _configuration = Configuration;
            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
            _mapper = mapper;
        }
    
        public async Task<IActionResult> UserList(string filter, int pageIndex = 1 , 
                                                                string sortExpression = "LastName")
        {
            var getUsersApiUrl = apiBaseUrl + "api/usermanager/getusers";           

            try
            {
                var jsonStr = await GetClient().GetStringAsync(getUsersApiUrl);
                var qry = JsonConvert.DeserializeObject<List<ApiUser>>(jsonStr).ToList();
                //   var qry = PagingList.Create(response, 6, pageIndex);
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry = qry.Where(p => (p.FirstName!=null&& p.FirstName.ToLower().Contains(filter.ToLower()))
                    || (p.Code!=null && p.Code.ToLower().Contains(filter.ToLower()))).ToList();
                }
                    var model = PagingList.Create(
                                         qry, 5, pageIndex, sortExpression, "LastName");
                model.Action = "UserList";
                model.RouteValue = new RouteValueDictionary { { "filter", filter } };

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private HttpClient GetClient()
        {
            string access_token = Request.Cookies["access_token"];
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
            return client;
        }
      
        private HttpContent getModelSringContent<T>(T model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }
        public async Task<IActionResult> Edit(string id)
        {
            var getUsersApiUrl = apiBaseUrl + "api/usermanager/getuser?id="+id;
            try
            {
                var jsonStr = await GetClient().GetStringAsync(getUsersApiUrl);
                var response = JsonConvert.DeserializeObject<ApiUser>(jsonStr);

                if (response==null)
                {
                    return NotFound();
                }
                var viewModel = _mapper.Map<ApiUser, EditViewModel>(response);
                viewModel.Image = response.Image!=null? ConvertApp.ToIFormFile(response.Image):null;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
                      
        }

     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id , EditViewModel model)
        {
            if (id!= model.Id )
            {
                return NotFound();
            }          
            if (ModelState.IsValid)
            {
                var apiUser = _mapper.Map<EditViewModel, ApiUser>(model);
                apiUser.Image = ConvertApp.ToBase64String(model.Image);
                var registerApiUrl = apiBaseUrl + "api/usermanager/update";
                var response = await GetClient().PostAsync(registerApiUrl, getModelSringContent(apiUser));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("UserList", "UserManager");
                }
                else
                {
                    if (response.StatusCode== System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    ModelState.Clear();
                    ModelState.AddModelError("UserName", $" خطا در ویرایش کاربر. کد خطا : {response.StatusCode}");
                    return View(model);
                }
            }

            return View(model);


        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var getUsersApiUrl = apiBaseUrl + "api/usermanager/getuser?id=" + id;
            try
            {
                var jsonStr = await GetClient().GetStringAsync(getUsersApiUrl);                
                var response = JsonConvert.DeserializeObject<ApiUser>(jsonStr);
                

                if (response == null)
                {
                    return NotFound();
                }
                var viewModel = _mapper.Map<ApiUser, EditViewModel>(response);                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
                return BadRequest();
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {         
            try
            {
                var deleteApiUrl = apiBaseUrl + "api/usermanager/delete/" + id;
                var response = await GetClient().DeleteAsync(deleteApiUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("UserList", "UserManager");
                }
                else
                {
                    throw new Exception ();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
               
            }  

        }
    }
}
