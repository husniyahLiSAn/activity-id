using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interface
{
    public interface ITokenRepository
    {
        public TokenVM Get(string username);
        public int Insert(TokenVM tokenVM);
        public int Update(TokenVM tokenVM);
    }
}
