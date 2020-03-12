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
    public class TokenService : ITokenService
    {
        private ITokenRepository _tokenRepository;
        public TokenService() { }

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public TokenVM Get(string username)
        {
            return _tokenRepository.Get(username);
        }

        public int Insert(TokenVM tokenVM)
        {
            return _tokenRepository.Insert(tokenVM);
        }

        public int Update(TokenVM tokenVM)
        {
            return _tokenRepository.Update(tokenVM);
        }
    }
}
