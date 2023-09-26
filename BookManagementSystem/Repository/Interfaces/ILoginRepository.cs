using BookManagementSystem.Common;

namespace BookManagementSystem.Repository.Interfaces
{
    public interface ILoginRepository
    {
        Task<ResultModel<UserModel>> Authenticate(UserLogin userLogin, string ipAddress);
        Task<ResultModel<AuthenticateResponse>> RefreshToken(string token, string ipAddress);
    }
}
