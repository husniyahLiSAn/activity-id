using API.Services.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserService : IUserService
    {
        int result = 0;

        private IUserRepository _userRepository;
        public UserService() { }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> Login(UserVM userVM)
        {
            return await _userRepository.Login(userVM);
        }

        public async Task<IdentityResult> Register(UserVM userVM)
        {
            return await _userRepository.Register(userVM);
        }

        public User GetUser(UserVM userVM)
        {
            return _userRepository.GetUser(userVM);
        }

        public int Create(UserVM userVM)
        {
            if (string.IsNullOrWhiteSpace(userVM.Name) || string.IsNullOrWhiteSpace(userVM.Username) || string.IsNullOrWhiteSpace(userVM.Password))
            {
                return result;
            }
            else
            {
                return _userRepository.Create(userVM);
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
                return _userRepository.Delete(Id);
            }
        }

        public int Update(UserVM userVM)
        {
            if (string.IsNullOrWhiteSpace(userVM.Name) || string.IsNullOrWhiteSpace(userVM.Username) || string.IsNullOrWhiteSpace(userVM.Password))
            {
                return result;
            }
            else
            {
                return _userRepository.Update(userVM);
            }
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _userRepository.Get();
        }

        public async Task<IEnumerable<User>> Get(int Id)
        {
            return await _userRepository.Get(Id);
        }
    }
}
