using System.Text.Json.Serialization;

namespace BookManagementSystem.Common
{
    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; } = string.Empty;
    }
}
