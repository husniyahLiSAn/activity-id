using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace Data.Repository
{
    public class ItemRepository : IItemRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        public readonly ConnectionStrings connectionString;

        public ItemRepository(ConnectionStrings _connectionStrings)
        {
            this.connectionString = _connectionStrings;
        }

        public int Create(ItemVM itemVM)
        {
            parameters.Add("@Name", itemVM.Name);
            parameters.Add("@Price", itemVM.Price);
            parameters.Add("@Stock", itemVM.Stock);
            parameters.Add("UserId", itemVM.Email);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var items = connection.Execute("SP_InsertItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return items;
            }
        }

        public async Task<IEnumerable<ItemVM>> Get()
        {
            var data = (IEnumerable<ItemVM>)null;
            try
            {
                const string procedure = "SP_GetItems";
                using (var connection = new SqlConnection(connectionString.Value))
                {
                    data = await connection.QueryAsync<ItemVM>(procedure);
                    //data = await connection.QueryAsync<ItemVM>(procedure, parameters, commandType: CommandType.StoredProcedure);
                    return data;
                }
                //using (var connection = new SqlConnection(connectionString.Value))
                //{
                //    data = await connection.QueryAsync<ItemVM>("SP_GetItems",
                //        parameters, commandType: CommandType.StoredProcedure);
                //    return data;
                //}
            }
            catch (Exception) { }
            return data;
        }

        public async Task<IEnumerable<ItemVM>> Get(int id)
        {
            parameters.Add("@Id", id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var items = await connection.QueryAsync<ItemVM>("SP_GetItemId",
                    parameters, commandType: CommandType.StoredProcedure);
                return items;
            }
            //    return myContext.ItemVMs.FromSqlRaw($"SP_GetItemId {id}");
        }

        public int Update(ItemVM itemVM)
        {
            parameters.Add("@Id", itemVM.Id);
            parameters.Add("@Name", itemVM.Name);
            parameters.Add("@Price", itemVM.Price);
            parameters.Add("@Stock", itemVM.Stock);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var items = connection.Execute("SP_UpdateItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return items;
            }
        }

        public int Delete(int id)
        {
            parameters.Add("@Id", id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var items = connection.Execute("SP_DeleteItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return items;
            }
        }

        public async Task<Paging> Paging(string userId, string keyword, int pageSize, int pageNumber)
        {
            var result = new Paging();
            try
            {
                parameters.Add("email", userId);
                parameters.Add("keyword", keyword);
                parameters.Add("pageSize", pageSize);
                parameters.Add("pageNumber", pageNumber);
                parameters.Add("length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("filteredLength", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var connection = new SqlConnection(connectionString.Value))
                {
                    result.dataItems = await connection.QueryAsync<ItemVM>("SP_PagingItems",
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
