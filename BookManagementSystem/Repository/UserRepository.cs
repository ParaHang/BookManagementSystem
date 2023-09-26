using BookManagementSystem.Common;
using BookManagementSystem.DBContext;
using BookManagementSystem.Entities;
using BookManagementSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace BookManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly IPasswordHasher<Users> _passwordHasher;
        private readonly UserManager<Users> _userManager;
        public UserRepository(ApplicationDbContext context, IPasswordHasher<Users> passwordHasher, UserManager<Users> userManager)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }
        public async Task<ResultModel<string>> CreateRoles(Roles roles)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                if (roles != null)
                {
                    if (roles.Id == null)
                    {
                        Guid id = Guid.NewGuid();
                        roles.Id = id.ToString();
                    }
                    _context.Roles.Add(roles);
                    await _context.SaveChangesAsync();

                    result.status = "00";
                    result.success = true;
                    result.message = "Role created successfully.";
                }
            }
            catch (Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<string>> CreateUser(AdminViewModel model)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                Users users = new Users()
                { 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NormalizedEmail = model.Email.ToUpper(),
                    NormalizedUserName = model.Email.ToUpper(),
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = model.Password,
                };
                if (model.Id == null)
                {
                    var guidId = Guid.NewGuid().ToString();
                    users.Id = guidId.ToString();
                }
                else
                {
                    users.Id = model.Id;
                }
                if(_context.Roles.Where(x=>x.Name == model.Role).Any())
                {
                    var message = await _userManager.CreateAsync(users, users.PasswordHash);
                    if (message.Succeeded)
                    {
                        var roleInsert = await _userManager.AddToRoleAsync(users, model.Role);
                        if (roleInsert.Succeeded)
                        {
                            result.status = "00";
                            result.success = true;
                            result.message = "User created successfully.";
                        }
                        else
                        {
                            result.message = "Failed to create User.";
                        }
                    }
                    else
                    {
                        foreach (var error in message.Errors)
                        {
                            result.message = error.Description;
                        }
                    }
                }
                else
                {
                    result.message = "Role not found.";
                }
                
            }
            catch (Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }
    }
}
