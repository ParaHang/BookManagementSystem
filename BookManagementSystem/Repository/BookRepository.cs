using BookManagementSystem.Common;
using BookManagementSystem.CommonModels;
using BookManagementSystem.DBContext;
using BookManagementSystem.Entities;
using BookManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<string>> Create(Book book)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                result.status = "00";
                result.success = true;
                result.message = "Operation Successful";

            }
            catch (Exception ex)
            { 
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<string>> Delete(int id)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();

                    result.status = "00";
                    result.success = true;
                    result.message = "Operation Successful";
                }
                else
                { 
                    result.message = "Books not found";
                }

            }
            catch (Exception ex)
            { 
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<Book>> Get(int id)
        {
            ResultModel<Book> result = new ResultModel<Book>();
            try
            {
                var book =  await _context.Books.Where(x=>x.Id == id).FirstOrDefaultAsync();
                if (book != null)
                {
                    result.status = "00";
                    result.success = true;
                    result.data.Add(book);
                    result.message = "Operation Successful";
                }
                else
                {
                    result.status = "00";
                    result.success = true;
                    result.message = "No Data";
                }
            }
            catch (Exception ex)
            { 
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<Book>> GetAll(PageParams pageParams)
        {
            ResultModel<Book> result = new ResultModel<Book>();
            try
            {
                var bookList = await _context.Books
                    .Skip((pageParams.PageNumber - 1) * (pageParams.PageSize))
                    .Take(pageParams.PageSize)
                    .ToListAsync();

                if (bookList != null && bookList.Any())
                {
                    result.status = "00";
                    result.success = true;
                    result.data = bookList;
                    result.message = "Operation Successful";
                }
                else
                {
                    result.status = "00";
                    result.success = true;
                    result.message = "No Data";
                }
            }
            catch (Exception ex)
            { 
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<string>> Update(Book book)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                var updateModel = await _context.Books.Where(x=>x.Id == book.Id).FirstOrDefaultAsync();
                if (updateModel!= null)
                {
                    updateModel.Author = book.Author;
                    updateModel.Title = book.Title;
                    updateModel.PublicationYear = book.PublicationYear;
                    updateModel.Genre = book.Genre;

                    _context.Books.Update(updateModel);
                    await _context.SaveChangesAsync();

                    result.status = "00";
                    result.success = true;
                    result.message = "Operation Successful";
                }
                else
                { 
                    result.message = "Books not found";
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
