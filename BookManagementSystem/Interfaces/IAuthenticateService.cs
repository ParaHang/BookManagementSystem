using BookManagementSystem.Common;

namespace BookManagementSystem.Interfaces
{
    public interface IAuthenticateService
    {
        string GenerateJwtToken(UserModel user);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        string generateRefreshToken(string ipAddress);
    }
}
