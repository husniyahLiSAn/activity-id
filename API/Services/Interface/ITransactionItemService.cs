using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface ITransactionItemService
    {
        Task<IEnumerable<TransactionItemVM>> Get(string email);
        Task<IEnumerable<TransactionItemVM>> Get(int id);
        Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber);
        int Create(TransactionItemVM transactionItemVM);
        int Update(TransactionItemVM transactionItemVM);
        int Pay(int Id);
        int Delete(int id);
    }
}
