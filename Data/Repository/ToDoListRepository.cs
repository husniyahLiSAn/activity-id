using Dapper;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ToDoListRepository : IToDoListRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        public readonly ConnectionStrings connectionString;

        public ToDoListRepository(ConnectionStrings _connectionStrings)
        {
            this.connectionString = _connectionStrings;
        }

        public int Create(ToDoListVM toDoListVM)
        {
            parameters.Add("@Name", toDoListVM.Name);
            parameters.Add("@Email", toDoListVM.Email);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_InsertToDoList",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Update(ToDoListVM toDoListVM)
        {
            parameters.Add("@Id", toDoListVM.Id);
            parameters.Add("@Name", toDoListVM.Name);
            //if (toDoListVM.Status == false)
            //{
            //    parameters.Add("@Status", 0);
            //}
            //else if (toDoListVM.Status == true)
            //{
            //    parameters.Add("@Status", 1);
            //}
            //parameters.Add("@Status", toDoListVM.Status);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_UpdateToDoList",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int UpdateStatus(int id)
        {
            parameters.Add("@Id", id);
            parameters.Add("@Status", 1);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_UpdateStatus",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int UncheckStatus(int id)
        {
            parameters.Add("@Id", id);
            parameters.Add("@Status", 0);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_UpdateStatus",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Delete(int Id)
        {
            parameters.Add("@Id", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_DeleteToDoList",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ToDoListVM>> GetAll(string Id, int Status)
        {
            var data = (IEnumerable<ToDoListVM>)null;
            try
            {
                parameters.Add("@Id", Id);
                parameters.Add("@Status", Status);
                using (var connection = new SqlConnection(connectionString.Value))
                {
                    data = await connection.QueryAsync<ToDoListVM>("SP_GetAllToDoLists",
                        parameters, commandType: CommandType.StoredProcedure);
                    return data;
                }
            }
            catch (Exception) { }
            return data;
        }
        public async Task<IEnumerable<ToDoListVM>> GetDone(string Id)
        {
            parameters.Add("@UserId", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<ToDoListVM>("SP_GetDoneToDoLists",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ToDoListVM>> GetUndone(string Id)
        {
            parameters.Add("@UserId", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<ToDoListVM>("SP_GetUnDoneToDoLists",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ToDoList>> Get(int Id)
        {
            parameters.Add("@Id", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<ToDoList>("SP_GetToDoListId",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ToDoListVM>> Search(string Id, int status, string keyword)
        {
            parameters.Add("userid", Id);
            parameters.Add("status", status);
            parameters.Add("keyword", keyword);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<ToDoListVM>("SP_Search",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<Paging> Paging(string Id, int status, string keyword, int pageSize, int pageNumber)
        {
            var result = new Paging();
            try
            {
                parameters.Add("email", Id);
                parameters.Add("status", status);
                parameters.Add("keyword", keyword);
                parameters.Add("pageSize", pageSize);
                parameters.Add("pageNumber", pageNumber);
                parameters.Add("length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("filteredLength", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var connection = new SqlConnection(connectionString.Value))
                {
                    result.dataActivities = await connection.QueryAsync<ToDoListVM>("SP_PagingActivities",
                        parameters, commandType: CommandType.StoredProcedure);
                    result.length = parameters.Get<int>("length");
                    result.filteredLength = parameters.Get<int>("filteredLength");
                    return result;
                }
            }
            catch (Exception) { }
            return result;
        }
    }
}
