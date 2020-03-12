using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ItemsController : Controller
    {
        readonly HttpClient client = new HttpClient();
        // GET: Item
        public ItemsController()
        {
            client.BaseAddress = new Uri("https://localhost:44334/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            var a = HttpContext.Session.GetString("Id");
            var b = HttpContext.Session.GetString("Name");
            if (a != null && b != null)
            {
                return View("Index");
            }
            return RedirectToAction("SignIn", "Home");
        }

        public JsonResult List()
        {
            IEnumerable<ItemVM> items = null;
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var responseTask = client.GetAsync("Items");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<ItemVM>>();
                readTask.Wait();
                items = readTask.Result;
            }
            else
            {
                items = Enumerable.Empty<ItemVM>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(items);
        }

        public async Task<Paging> Paging(string keyword, int pageSize, int pageNumber)
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("items/paging?userId=" + HttpContext.Session.GetString("Id") + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<Paging>();
                }
            }
            catch (Exception) { }
            return null;
        }

        //[HttpGet("Items/PageData")]
        public IActionResult PageData(IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / pageSize + 1;
            var keyword = request.Search.Value;
            var dataPage = Paging(keyword, pageSize, pageNumber).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filteredLength, dataPage.dataItems);
            return new DataTablesJsonResult(response, true);

        }

        public JsonResult GetbyID(int Id)
        {
            IEnumerable<ItemVM> item = null;
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var responseTask = client.GetAsync("Items/ " + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<ItemVM>>();
                readTask.Wait();
                item = readTask.Result;
            }
            else
            {
                item = Enumerable.Empty<ItemVM>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(item);
        }

        // GET: Employees/Create
        public JsonResult Insert(ItemVM itemVM)
        {
            itemVM.Email = HttpContext.Session.GetString("Id");
            var myContent = JsonConvert.SerializeObject(itemVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = client.PostAsync("Items", byteContent).Result;
            return Json(result);
        }

        // GET: Items/Edit/5
        public JsonResult Update(ItemVM itemVM)
        {
            var myContent = JsonConvert.SerializeObject(itemVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = client.PutAsync("Items/" + itemVM.Id, byteContent).Result;
            return Json(result);
        }

        // GET: Items/Delete/5
        public JsonResult Delete(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = client.DeleteAsync("Items/" + id).Result;
            return Json(result);
        }
    }
}