using API.Services.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ToDoListService : IToDoListService
    {
        int result = 0;

        private IToDoListRepository _toDoListRepository;
        public ToDoListService() { }

        public ToDoListService(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        public int Create(ToDoListVM toDoListVM)
        {
            if (string.IsNullOrWhiteSpace(toDoListVM.Name))
            {
                return result;
            }
            else
            {
                return _toDoListRepository.Create(toDoListVM);
            }
        }
        public int Update(ToDoListVM toDoListVM)
        {
            if (string.IsNullOrWhiteSpace(toDoListVM.Name))
            {
                return result;
            }
            else
            {
                return _toDoListRepository.Update(toDoListVM);
            }
        }

        public int UpdateStatus(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return result;
            }
            else
            {
                return _toDoListRepository.UpdateStatus(id);
            }
        }

        public int UncheckStatus(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return result;
            }
            else
            {
                return _toDoListRepository.UncheckStatus(id);
            }
        }

        public int Delete(int Id)
        {
            if (string.IsNullOrWhiteSpace(Id.ToString()))
            {
                return result;
            }
            else
            {
                return _toDoListRepository.Delete(Id);
            }
        }

        public async Task<IEnumerable<ToDoListVM>> GetAll(string Id, int Status)
        {
            return await _toDoListRepository.GetAll(Id, Status);
        }

        public async Task<IEnumerable<ToDoListVM>> GetDone(string Id)
        {
            return await _toDoListRepository.GetDone(Id);
        }

        public async Task<IEnumerable<ToDoListVM>> GetUndone(string Id)
        {
            return await _toDoListRepository.GetUndone(Id);
        }

        public async Task<IEnumerable<ToDoList>> Get(int Id)
        {
            return await _toDoListRepository.Get(Id);
        }

        public async Task<IEnumerable<ToDoListVM>> Search(string Id, int Status, string Keyword)
        {
            return await _toDoListRepository.Search(Id, Status, Keyword);
        }

        public async Task<Paging> Paging(string Id, int status, string keyword, int pageSize, int pageNumber)
        {
            return await _toDoListRepository.Paging(Id, status, keyword, pageSize, pageNumber);
        }
    }
}
