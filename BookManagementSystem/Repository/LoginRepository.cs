using BookManagementSystem.Common;
using BookManagementSystem.DBContext;
using BookManagementSystem.Entities;
using BookManagementSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookManagementSystem.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;
        public IConfiguration _config;
        public LoginRepository(UserManager<Users> userManager, ApplicationDbContext context, IConfiguration config)
        {
            _config = config;
            _userManager = userManager; 
            _context = context;
        }
        public async Task<ResultModel<UserModel>> Authenticate(UserLogin userLogin, string ipAddress)
        {
            ResultModel<UserModel> result = new ResultModel<UserModel>();
            try
            {
                var userNew = new UserModel();
                var user = await _userManager.FindByNameAsync(userLogin.Username);
                var refreshToken = generateRefreshToken(ipAddress);
                if (user == null)
                {
                    result.message = "User not found.";
                }
                else
                {
                    var isValidUser = await _userManager.CheckPasswordAsync(user, userLogin.Password);
                    if (isValidUser)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToInt32(_config["RefreshTokenExpiryInDays"]));
                        _context.Users.Update(user);
                        _context.SaveChanges();

                        userNew.EmailAddress = user.Email;
                        userNew.GivenName = user.FirstName;
                        userNew.Surname = user.LastName;
                        userNew.Role = roles[0];
                        userNew.Username = user.Email;
                        userNew.Id = user.Id;
                        userNew.RefreshToken = refreshToken;

                        result.message = "Operation Successful.";
                        result.success = true;
                        result.status = "00";
                        result.data.Add(userNew);
                    }
                    else
                    {
                        result.message = "Invalid Credentials.";
                    } 
                }
            }
            catch(Exception ex)
            {
                result.message = "Internal Server error: " + ex.Message;
            }
            return result;
        }
        public async Task<ResultModel<AuthenticateResponse>> RefreshToken(string token, string ipAddress)
        {
            ResultModel<AuthenticateResponse> result = new ResultModel<AuthenticateResponse>();
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == token);
                // return null if no user found with token
                if (user == null)
                {
                    result.message = "User not found.";
                }
                else
                {
                    var refreshToken = user.RefreshToken;

                    // return null if token is no longer active
                    if (refreshToken == null)
                    {
                        result.message = "Refresh token is expired.";
                    }
                    else
                    {
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

                        result.status = "00";
                        result.success = true;
                        result.message = "Operation Successful.";
                        result.data.Add(new AuthenticateResponse(user, jwtToken, newRefreshToken));
                    }
                }
            }
            catch(Exception ex) 
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }
        private string generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
        private string GenerateJwtToken(UserModel user)
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
