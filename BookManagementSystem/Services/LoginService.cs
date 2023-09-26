using BookManagementSystem.Common;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookManagementSystem.Services
{
    public class LoginService: ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<ResultModel<UserModel>> Authenticate(UserLogin userLogin, string ipAddress)
        {
            return await _loginRepository.Authenticate(userLogin, ipAddress);
        }

        public async Task<ResultModel<AuthenticateResponse>> RefreshToken(string token, string ipAddress)
        {
            return await _loginRepository.RefreshToken(token, ipAddress);
        }
    }
}
