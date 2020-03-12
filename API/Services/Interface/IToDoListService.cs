using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface IToDoListService
    {
        //Task<IEnumerable<ToDoListVM>> GetAll(ToDoListVM toDoListVM);
        Task<IEnumerable<ToDoListVM>> GetAll(string Id, int Status);
        Task<IEnumerable<ToDoListVM>> GetDone(string Id);
        Task<IEnumerable<ToDoListVM>> GetUndone(string Id);
        Task<IEnumerable<ToDoList>> Get(int Id);
        Task<IEnumerable<ToDoListVM>> Search(string Id, int status, string keyword);
        Task<Paging> Paging(string Id, int status, string keyword, int pageSize, int pageNumber);
        int Create(ToDoListVM toDoListVM);
        int Update(ToDoListVM toDoListVM);
        int UpdateStatus(int id);
        int UncheckStatus(int id);
        int Delete(int Id);
    }
}
