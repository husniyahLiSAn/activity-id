using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface IItemRepository
    {
        Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber);
        Task<IEnumerable<ItemVM>> Get();
        Task<IEnumerable<ItemVM>> Get(int id);
        int Create(ItemVM itemVM);
        int Update(ItemVM itemVM);
        int Delete(int id);
    }
}
