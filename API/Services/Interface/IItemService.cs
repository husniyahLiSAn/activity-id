using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Services.Interface
{
    public interface IItemService
    {
        Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber);
        Task<IEnumerable<ItemVM>> Get();
        Task<IEnumerable<ItemVM>> Get(int id);
        int Create(ItemVM itemVM);
        int Update(ItemVM itemVM);
        int Delete(int id);
    }
}