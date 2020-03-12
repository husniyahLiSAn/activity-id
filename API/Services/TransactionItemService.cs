using API.Services.Interface;
using Data.Context;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TransactionItemService : ITransactionItemService
    {
        int status = 0;

        private ITransactionItemRepository _transactionItemRepository;

        public TransactionItemService(ITransactionItemRepository TransactionItemRepository)
        {
            _transactionItemRepository = TransactionItemRepository;
        }

        public int Create(TransactionItemVM transactionItemVM)
        {
            if (string.IsNullOrWhiteSpace(transactionItemVM.ItemId.ToString()))
            {
                return status;
            }
            else
            {
                return _transactionItemRepository.Create(transactionItemVM);
            }
        }

        public int Delete(int id)
        {
            return _transactionItemRepository.Delete(id);
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(int id)
        {
            return await _transactionItemRepository.Get(id);
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(string email)
        {
            return await _transactionItemRepository.Get(email);
        }

        public async Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber)
        {
            return await _transactionItemRepository.Paging(userId, keyword, pageSize, pageNumber);
        }

        public int Pay(int Id)
        {
            if (string.IsNullOrWhiteSpace(Id.ToString()))
            {
                return status;
            }
            else
            {
                return _transactionItemRepository.Pay(Id);
            }
        }

        public int Update(TransactionItemVM transactionItemVM)
        {
            if (string.IsNullOrWhiteSpace(transactionItemVM.ItemId.ToString()))
            {
                return status;
            }
            else
            {
                return _transactionItemRepository.Update(transactionItemVM);
            }
        }
    }
}
