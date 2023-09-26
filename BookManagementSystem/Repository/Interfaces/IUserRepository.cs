using BookManagementSystem.Common;
using BookManagementSystem.Entities;

namespace BookManagementSystem.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ResultModel<string>> CreateUser(AdminViewModel user);
        Task<ResultModel<string>> CreateRoles(Roles roles);
    }
}
