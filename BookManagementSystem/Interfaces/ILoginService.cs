using BookManagementSystem.Common;

namespace BookManagementSystem.Interfaces
{
    public interface ILoginService
    {
        Task<ResultModel<UserModel>> Authenticate(UserLogin userLogin, string ipAddress);
        Task<ResultModel<AuthenticateResponse>> RefreshToken(string token, string ipAddress);
    }
}
