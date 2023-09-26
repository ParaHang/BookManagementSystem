using BookManagementSystem.Common;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Repository.Interfaces;

namespace BookManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResultModel<string>> CreateRole(Roles role)
        {
            return await _userRepository.CreateRoles(role);
        }

        public async Task<ResultModel<string>> CreateUser(AdminViewModel user)
        {
            return await _userRepository.CreateUser(user);
        }
    }
}
