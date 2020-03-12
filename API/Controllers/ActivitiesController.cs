using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        IToDoListService _toDoListService;

        public ActivitiesController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        // GET: api/ToDo
        [HttpGet]
        [Route("GetAll/{Id}/{Status}")]
        public async Task<ActionResult<IEnumerable<ToDoListVM>>> GetAll(string Id, int Status)
        {
            var data = await _toDoListService.GetAll(Id, Status);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        [HttpGet]
        [Route("Search/{Id}/{Status}/{Keyword}")]
        public async Task<IEnumerable<ToDoListVM>> Search(string Id, int Status, string Keyword)
        {
            Keyword = Keyword.Substring(1);
            return await _toDoListService.Search(Id, Status, Keyword);
        }

        //[HttpGet]
        //[Route("Paging/{Id}/{Status}/{Keyword}/{PageSize}/{PageNumber}")]
        //public async Task<IEnumerable<ToDoListVM>> Paging(string Id, int Status, string Keyword, int PageSize, int PageNumber)
        //{
        //    return await _toDoListService.Paging(Id, Status, Keyword, PageSize, PageNumber);
        //}
        [HttpGet]
        [Route("Paging")]
        public async Task<ActionResult<Paging>> Paging(string Id, int Status, string Keyword, int PageSize, int PageNumber)
        {
            if (Keyword==null)
            {
                Keyword = "";
            }
            var data = await _toDoListService.Paging(Id, Status, Keyword, PageSize, PageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        [HttpGet]
        [Route("GetDone/{Id}")]
        public async Task<ActionResult<IEnumerable<ToDoListVM>>> GetDone(string Id)
        {
            var data = await _toDoListService.GetDone(Id);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        [HttpGet]
        [Route("GetUndone/{Id}")]
        public async Task<ActionResult<IEnumerable<ToDoListVM>>> GetUndone(string Id)
        {
            var data = await _toDoListService.GetUndone(Id);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        // GET: api/ToDoLists/5
        [HttpGet("{id}")]
        //[Route("GetId")]
        public async Task<IEnumerable<ToDoList>> Get(int id)
        {
            return await _toDoListService.Get(id);
        }

        // POST: api/ToDoLists
        [HttpPost]
        public IActionResult Post(ToDoListVM toDoListVM)
        {
            var data = _toDoListService.Create(toDoListVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }

        // PUT: api/ToDoLists/5
        [HttpPut("{id}")]
        public IActionResult Put(ToDoListVM toDoListVM)
        {
            var data = _toDoListService.Update(toDoListVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }

        [HttpPut]
        [Route("UpdateStatus/{Id}")]
        public IActionResult UpdateStatus(int Id)
        {
            var data = _toDoListService.UpdateStatus(Id);
            if (data < 0)
            {
                return NotFound("No Data Found");
            }
            return Ok(data);
        }

        [HttpPut]
        [Route("UncheckStatus/{Id}")]
        public IActionResult UncheckStatus(int id)
        {
            var data = _toDoListService.UncheckStatus(id);
            if (data < 0)
            {
                return NotFound("No Data Found");
            }
            return Ok(data);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _toDoListService.Delete(id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }


    }
}