using Dapper;
using Data.Context;
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
    public class TransactionItemRepository : ITransactionItemRepository
    {
        MyContext myContext = new MyContext();
        DynamicParameters parameters = new DynamicParameters();
        public readonly ConnectionStrings connectionString;

        public TransactionItemRepository(ConnectionStrings _connectionStrings)
        {
            this.connectionString = _connectionStrings;
        }

        public int Create(TransactionItemVM transactionItemVM)
        {
            parameters.Add("@Quantity", transactionItemVM.Quantity);
            parameters.Add("@UserId", transactionItemVM.Email);
            parameters.Add("@Item", transactionItemVM.ItemId);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_InsertTransItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Delete(int id)
        {
            parameters.Add("@TransItem", id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_DeleteTransItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(string email)
        {
            const string procedure = "SP_CheckTransaction";
            var data = (IEnumerable<TransactionItemVM>)null;
            parameters.Add("@Email", email);
            try
            {
                using (var connection = new SqlConnection(connectionString.Value))
                {
                    data = await connection.QueryAsync<TransactionItemVM>(procedure, parameters, commandType: CommandType.StoredProcedure);
                    return data;
                }
            }
            catch (Exception) { }
            return data;
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(int id)
        {
            const string procedure = "SP_GetTransItemId";
            var data = (IEnumerable<TransactionItemVM>)null;
            parameters.Add("Id", id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                data = await connection.QueryAsync<TransactionItemVM>(procedure, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Update(TransactionItemVM transactionItemVM)
        {
            parameters.Add("@Quantity", transactionItemVM.Quantity);
            parameters.Add("@TransItem", transactionItemVM.TransactionId);
            parameters.Add("@Item", transactionItemVM.ItemId);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_UpdateTransItem",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Pay(int Id)
        {
            parameters.Add("Id", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_Pay",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
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
                    result.dataTransactionItems = await connection.QueryAsync<TransactionItemVM>("SP_PagingTransactions",
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
