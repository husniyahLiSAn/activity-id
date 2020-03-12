using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("Paging")]
        public async Task<ActionResult<Paging>> Paging(string userId, string Keyword, int PageSize, int PageNumber)
        {
            if (Keyword == null)
            {
                Keyword = "";
            }
            var data = await _itemService.Paging(userId, Keyword, PageSize, PageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        [HttpGet]
        public async Task<IEnumerable<ItemVM>> Get()
        {
            return await _itemService.Get();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ItemVM>> Get(int id)
        {
            return await _itemService.Get(id);
        }

        [HttpPost]
        public IActionResult Post(ItemVM itemVM)
        {
            var data = _itemService.Create(itemVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
            //var status = Ok(data);
        }

        [HttpPut("{id}")]
        public IActionResult Put(ItemVM itemVM)
        {
            var data = _itemService.Update(itemVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var data = _itemService.Delete(Id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }
    }
}