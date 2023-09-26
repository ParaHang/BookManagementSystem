using BookManagementSystem.Common;
using BookManagementSystem.Entities;

namespace BookManagementSystem.Interfaces
{
    public interface IUserService
    {
        Task<ResultModel<string>> CreateUser(AdminViewModel user);
        Task<ResultModel<string>> CreateRole(Roles role);
    }
}
