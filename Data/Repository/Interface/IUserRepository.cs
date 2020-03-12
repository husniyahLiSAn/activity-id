using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();
        Task<IEnumerable<User>> Get(int Id);
        public Task<IEnumerable<User>> Login(UserVM userVM);
        public Task<IdentityResult> Register(UserVM userVM);
        User GetUser(UserVM userVM);
        int Create(UserVM userVM);
        int Update(UserVM userVM);
        int Delete(int Id);
    }
}
