using BookManagementSystem.Common;
using BookManagementSystem.DBContext;
using BookManagementSystem.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookManagementSystem.Services
{
    public class AuthenticateServices : IAuthenticateService
    {
        public IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public AuthenticateServices(ApplicationDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshToken == token);

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshToken;

            // return null if token is no longer active
            if (refreshToken == null) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            user.RefreshToken = newRefreshToken;
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = GenerateJwtToken(new UserModel
            {
                EmailAddress = user.Email,
                GivenName = user.FirstName,
                Surname = user.LastName,
                Role = "Admin",
                Username = user.Email,
                Id = user.Id,
            });

            return new AuthenticateResponse(user, jwtToken, newRefreshToken);
        }
        public string generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
        public string GenerateJwtToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmailAddress),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(1200),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }

    }
}
