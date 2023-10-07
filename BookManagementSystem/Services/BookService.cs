using BookManagementSystem.Common;
using BookManagementSystem.CommonModels;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Repository.Interfaces;
using NuGet.Protocol;

namespace BookManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<ResultModel<string>> Create(Book book)
        {
            return await _bookRepository.Create(book);
        }

        public Task<ResultModel<string>> Delete(int id)
        {
            return _bookRepository.Delete(id);
        }

        public Task<ResultModel<Book>> Get(int id)
        {
            return _bookRepository.Get(id);
        }

        public async Task<ResultModel<Book>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

        public Task<ResultModel<string>> Update(Book book)
        {
            return _bookRepository.Update(book);
        }
        public async Task<ResultModel<Book>> GetBooksByIds(List<string> ids)
        {
            return await _bookRepository.GetBooksByIds(ids);
        }
    }
}
