using Dapper;
using Data.Context;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        DynamicParameters parameters = new DynamicParameters();
        public readonly ConnectionStrings connectionString;


        public UserRepository(ConnectionStrings _connectionStrings, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.connectionString = _connectionStrings;
        }

        public async Task<IEnumerable<User>> Login(UserVM userVM)
        {
            var data = (IEnumerable<User>)null;

            try
            {
                var result = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);
                if (result.Succeeded)
                {
                    parameters.Add("@Username", userVM.Username);
                    parameters.Add("@Password", userVM.Password);
                    //parameters.Add("@Token", userVM.Token);
                    using (var con = new SqlConnection(connectionString.Value))
                    {
                        data = await con.QueryAsync<User>("SP_Login",
                            parameters, commandType: CommandType.StoredProcedure);
                        return data;
                    }
                }
            }
            catch (Exception) { }
            return data;
        }

        public async Task<IdentityResult> Register(UserVM userVM)
        {
            var user = new User(userVM);
            var result = await _userManager.CreateAsync(user, userVM.Password);
            return result;
        }

        public User GetUser(UserVM userVM)
        {
            parameters.Add("@Username", userVM.Username);
            parameters.Add("@Password", userVM.Password);
            //parameters.Add("@Token", userVM.Token);
            using (var con = new SqlConnection(connectionString.Value))
            {
                var data = con.Query<User>("SP_Login",
                    parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return data;
            }
        }

        public int Create(UserVM userVM)
        {
            parameters.Add("@Name", userVM.Name);
            parameters.Add("@Username", userVM.Username);
            parameters.Add("@Password", userVM.Password);
            //parameters.Add("@Token", userVM.Token);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_InsertUser",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Update(UserVM userVM)
        {
            parameters.Add("@Id", userVM.Id);
            parameters.Add("@Name", userVM.Name);
            parameters.Add("@Username", userVM.Username);
            parameters.Add("@Password", userVM.Password);
            //parameters.Add("@Token", userVM.Token);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_UpdateUser",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public int Delete(int Id)
        {
            parameters.Add("@Id", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = connection.Execute("SP_DeleteUser",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<User>> Get()
        {
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<User>("SP_GetUsers");
                return data;
            }
        }

        public async Task<IEnumerable<User>> Get(int Id)
        {
            parameters.Add("@Id", Id);
            using (var connection = new SqlConnection(connectionString.Value))
            {
                var data = await connection.QueryAsync<User>("SP_GetUserId",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
