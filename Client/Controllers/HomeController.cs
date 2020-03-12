using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Client.Reports;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        readonly HttpClient client = new HttpClient();
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;

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
            return RedirectToAction("SignIn");
        }

        public ActionResult Kendo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            var a = HttpContext.Session.GetString("Id");
            var b = HttpContext.Session.GetString("Name");
            //var c = HttpContext.Session.GetString("Token");
            if (a != null && b != null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            var a = HttpContext.Session.GetString("Id");
            var b = HttpContext.Session.GetString("Name");
            //var c = HttpContext.Session.GetString("Token");
            if (a != null && b != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var a = HttpContext.Session.GetString("Id");
            var b = HttpContext.Session.GetString("Name");
            //var c = HttpContext.Session.GetString("Token");
            if (a == null && b == null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                var signIn = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (signIn.Succeeded)
                {
                    #region SetToken
                    var myContent = JsonConvert.SerializeObject(loginVM);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = client.PostAsync("users/login/", byteContent).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        #region Get Login API
                        //var user = result.Content.ReadAsStringAsync().Result.Replace("\"", "").Split("...");
                        //HttpContext.Session.SetString("Token", "Bearer " + user[0]);
                        //HttpContext.Session.SetString("Id", user[1]);
                        //HttpContext.Session.SetString("Name", user[2]);
                        #endregion
                        var user = result.Content.ReadAsAsync<TokenVM>().Result;
                        HttpContext.Session.SetString("Id", user.Email);
                        HttpContext.Session.SetString("Username", user.Username);
                        HttpContext.Session.SetString("Name", user.Name);
                        HttpContext.Session.SetString("Token", "Bearer " + user.AccessToken);
                        HttpContext.Session.SetString("ExpToken", user.ExpireToken.ToString());
                        HttpContext.Session.SetString("RefreshToken", user.RefreshToken);
                        HttpContext.Session.SetString("ExpRefreshToken", user.ExpireRefreshToken.ToString());

                        client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Bearer "+token);
                        var a = HttpContext.Session.GetString("Id");
                        //var b = HttpContext.Session.GetString("Name");
                        return RedirectToAction("Index");
                    }
                    #endregion
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Employees/Create
        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public ActionResult SignUp(UserVM userVM)
        {
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("users/register/", byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("SignIn");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult SignOut()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            var result = client.GetAsync("users/logout").Result;
            if (result.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("Id");
                HttpContext.Session.Remove("Username");
                HttpContext.Session.Remove("Name");
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("ExpToken");
                HttpContext.Session.Remove("RefreshToken");
                HttpContext.Session.Remove("ExpRefreshToken");

                HttpContext.Session.Clear();
                return RedirectToAction("SignIn");
            }
            return View();
        }


        [HttpGet("Home/ListAll/{status}")]
        //[Route("ListAll/{status}")]
        public async Task<IActionResult> ListAll(int status)
        {
            string Id = HttpContext.Session.GetString("Id");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            IEnumerable<ToDoListVM> activities = null;
            var responseTask = await client.GetAsync("activities/GetAll/" + Id + "/" + status);
            //var responseTask = client.GetAsync("activities/GetAll/" + _toDoListVM);
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();
                return Ok(new { data = readTask });
            }
            else
            {
                activities = Enumerable.Empty<ToDoListVM>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(activities);
        }

        public async Task<IEnumerable<ToDoListVM>> Search(int status, string keyword){
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("activities/search/" + HttpContext.Session.GetString("Id") + "/" + status + "/" + keyword);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<ToDoListVM>>();
                }
            }
            catch (Exception) { }
            return null;
        }

        public async Task RefreshToken(TokenVM tokenVM)
        {
            var myContent = JsonConvert.SerializeObject(tokenVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("users/refresh", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var user = await result.Content.ReadAsAsync<TokenVM>();
                HttpContext.Session.SetString("Id", user.Email);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Token", "Bearer " + user.AccessToken);
                HttpContext.Session.SetString("ExpToken", user.ExpireToken.ToString());
                HttpContext.Session.SetString("RefreshToken", user.RefreshToken);
                HttpContext.Session.SetString("ExpRefreshToken", user.ExpireRefreshToken.ToString());
            }
        }

        public async Task<Paging> Paging(int status, string keyword, int pageSize, int pageNumber)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            try
            {
                var response = await client.GetAsync("activities/paging?Id=" + HttpContext.Session.GetString("Id") + "&status=" + status + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<Paging>();
                }
            }
            catch (Exception) { }
            return null;
        }

        
        [HttpGet("Home/PageData/{status}")]
        [Authorize(Roles = "Seller, Customer")]
        public IActionResult PageData(IDataTablesRequest request, int status)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / pageSize + 1;
            var keyword = request.Search.Value;
            //var data = Search(status, keyword).Result;
            //var filteredData = data;
            var dataPage = Paging(status, keyword, pageSize, pageNumber).Result;
            //var sortable = dataPage.
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filteredLength, dataPage.dataActivities);
            return new DataTablesJsonResult(response, true);
        }

        public async Task<JsonResult> GetbyID(int Id)
        {
            IEnumerable<ToDoListVM> toDoList = null;
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var responseTask = client.GetAsync("activities/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<ToDoListVM>>();
                readTask.Wait();
                toDoList = readTask.Result;
            }
            else
            {
                toDoList = Enumerable.Empty<ToDoListVM>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(toDoList);
        }

        // GET: Employees/Create
        public async Task<JsonResult> Insert(ToDoListVM toDoListVM)
        {
            toDoListVM.Email = HttpContext.Session.GetString("Id");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var myContent = JsonConvert.SerializeObject(toDoListVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("activities", byteContent).Result;
            return Json(result);
        }

        // GET: activities/Edit/5
        public async Task<JsonResult> Update(ToDoListVM toDoListVM)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var myContent = JsonConvert.SerializeObject(toDoListVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("activities/" + toDoListVM, byteContent).Result;
            return Json(result);
        }

        public async Task<JsonResult> UpdateStatus(int Id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var myContent = JsonConvert.SerializeObject(Id);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("activities/updateStatus/" + Id, byteContent).Result;
            return Json(result);
        }

        public async Task<JsonResult> UncheckStatus(int Id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var myContent = JsonConvert.SerializeObject(Id);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("activities/uncheckStatus/" + Id, byteContent).Result;
            return Json(result);
        }

        // GET: activities/Delete/5
        public async Task<JsonResult> Delete(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var result = client.DeleteAsync("activities/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            var readTask = await GetActivities();
            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "Status",
                "Completed"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new works/heet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("All Activities"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                var k = 1;
                var j = 2;
                foreach (var activities in readTask)
                {
                    worksheet.Cells["A" + j].Value = k;
                    worksheet.Cells["B" + j].Value = activities.Name;
                    if (activities.Status==false)
                    {
                        worksheet.Cells["C" + j].Value = "Active";
                        worksheet.Cells["D" + j].Value = "-";
                    }
                    else
                    {
                        worksheet.Cells["C" + j].Value = "Completed";
                        worksheet.Cells["D" + j].Value = activities.CompletedTime.ToString("MM/dd/yyyy");
                    }
                    //worksheet.Cells["D" + j].Value = activities.Salary.ToString("$#,0.00;($#,0.00)");
                    //worksheet.Cells["E" + j].Value = activities.JoinedDate.ToString("MM/dd/yyyy");

                    k++;  j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Activities{DateTime.Now.ToString("MM/dd/yyyy")}.xlsx");
        }

        public async Task<IActionResult> CSV()
        {
            var readTask = await GetActivities();
            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "Status",
                "Completed"
            };

            var activitiesRecords = (from activities in readTask
                                   select new object[]
                                   {
                                            activities.Id,
                                            $"{activities.Name}",
                                            $"{activities.Status}",
                                            activities.CompletedTime.ToString("MM/dd/yyyy")
                                   }).ToList();

            // Build the file content
            var activitiescsv = new StringBuilder();
            activitiesRecords.ForEach(line =>
            {
                activitiescsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{activitiescsv.ToString()}");
            return File(buffer, "text/csv", $"Activities{DateTime.Now.ToString("MMddyyyy")}.csv");
        }

        public async Task<IActionResult> Report(ToDoListVM toDoListVM)
        {
            ActivitiesReport activities = new ActivitiesReport();
            var readTask = await GetActivities();
            byte[] abytes = activities.PrepareReport(readTask);
            return File(abytes, "application/pdf", $"ReportActivities{DateTime.Now.ToString("MMddyyyy")}.pdf");
        }

        public async Task<List<ToDoListVM>> GetActivities()
        {
            List<ToDoListVM> activities = new List<ToDoListVM>();
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            TokenVM tokenVM = new TokenVM();

            tokenVM.Username = HttpContext.Session.GetString("Username");
            tokenVM.ExpireToken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            tokenVM.RefreshToken = HttpContext.Session.GetString("RefreshToken");
            tokenVM.ExpireRefreshToken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefreshToken"));

            if (tokenVM.ExpireToken < DateTime.UtcNow.Ticks && tokenVM.ExpireRefreshToken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenVM);
            }
            else if (tokenVM.ExpireRefreshToken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            var responseTask = await client.GetAsync("activities/GetAll/" + HttpContext.Session.GetString("Id") + "/" + 3);
            activities = await responseTask.Content.ReadAsAsync<List<ToDoListVM>>();
            return activities;
        }
    }
}