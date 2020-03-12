using API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Model;
using Data.ViewModel;
using Data.Context;
using Data.Repository.Interface;
using System.Threading.Tasks;

namespace API.Services
{
    public class ItemService : IItemService
    {
        int status = 0;

        private IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public int Create(ItemVM itemVM)
        {
            if (string.IsNullOrWhiteSpace(itemVM.Name))
            {
                return status;
            }
            else
            {
                return _itemRepository.Create(itemVM);
            }
        }

        public int Delete(int id)
        {
            return _itemRepository.Delete(id);
        }

        public async Task<IEnumerable<ItemVM>> Get(int id)
        {
            return await _itemRepository.Get(id);
        }

        public async Task<IEnumerable<ItemVM>> Get()
        {
            return await _itemRepository.Get();
        }

        public async Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber)
        {
            return await _itemRepository.Paging(userId, keyword, pageSize, pageNumber);
        }

        public int Update(ItemVM itemVM)
        {
            if (string.IsNullOrWhiteSpace(itemVM.Name))
            {
                return status;
            }
            else
            {
                return _itemRepository.Update(itemVM);
            }
        }
    }
}