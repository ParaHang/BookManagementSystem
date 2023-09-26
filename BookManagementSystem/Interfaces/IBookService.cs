using BookManagementSystem.Common;
using BookManagementSystem.CommonModels;
using BookManagementSystem.Entities;

namespace BookManagementSystem.Interfaces
{
    public interface IBookService
    {
        Task<ResultModel<Book>> GetAll(PageParams pageParams);
        Task<ResultModel<Book>> Get(int id);
        Task<ResultModel<string>> Update(Book book);
        Task<ResultModel<string>> Create(Book book);
        Task<ResultModel<string>> Delete(int id); 
    }
}
