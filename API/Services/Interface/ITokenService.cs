using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface ITokenService
    {
        public TokenVM Get(string username);
        public int Insert(TokenVM tokenVM);
        public int Update(TokenVM tokenVM);
    }
}
