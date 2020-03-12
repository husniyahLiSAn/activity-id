using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer ")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionItemsController : ControllerBase
    {
        ITransactionItemService _transactionItemService;

        public TransactionItemsController(ITransactionItemService transactionItemService)
        {
            _transactionItemService = transactionItemService;
        }

        [HttpGet]
        [Route("Paging")]
        public async Task<ActionResult<Paging>> Paging(string userId, string Keyword, int PageSize, int PageNumber)
        {
            if (Keyword == null)
            {
                Keyword = "";
            }
            var data = await _transactionItemService.Paging(userId, Keyword, PageSize, PageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Failed ");
        }

        // GET: api/TransactionItems
        [HttpGet]
        [Route("GetPay/{email}")]
        public async Task<IEnumerable<TransactionItemVM>> GetPay(string email)
        {
            return await _transactionItemService.Get(email);
        }

        // GET: api/TransactionItems/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<TransactionItemVM>> Get(int id)
        {
            return await _transactionItemService.Get(id);
        }

        // POST: api/TransactionItems
        [HttpPost]
        public IActionResult Post(TransactionItemVM transactionItemVM)
        {
            var data = _transactionItemService.Create(transactionItemVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
            //var status = Ok(data);
        }

        [HttpPut("{id}")]
        public IActionResult Put(TransactionItemVM transactionItemVM)
        {
            var data = _transactionItemService.Update(transactionItemVM);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }

        [HttpPut]
        [Route("Pay/{Id}")]
        public IActionResult Pay(int Id)
        {
            var data = _transactionItemService.Pay(Id);
            if (data < 0)
            {
                return NotFound("No Data Found");
            }
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var data = _transactionItemService.Delete(Id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Failed");
        }
    }
}
