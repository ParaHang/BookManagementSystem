using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Entities;
using BookManagementSystem.Common;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using BookManagementSystem.Interfaces;
using BookManagementSystem.CommonModels;

namespace BookManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet] 
        [Authorize(Roles = "User, Admin")] 
        public async Task<ActionResult> GetBooks(int pageNumber = 1, int pageSize = 5)
        {
            PageParams pageParam = new PageParams()
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            };
            var books = await _bookService.GetAll(pageParam);
            return StatusCode((int)HttpStatusCode.OK, books);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> GetBook(int id)
        {
            if (id > 0)
            {
                var book = await _bookService.Get(id);
                return StatusCode((int)HttpStatusCode.OK, book);
            }
            else
                return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // PUT: api/Books/{id}
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutBook(Book book)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.Update(book);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        // POST: api/Books
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> PostBook(Book book)
        {

            if (book != null && ModelState.IsValid)
            {
                var result = await _bookService.Create(book);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/Books/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(int id)
        {
             if( id > 0 )
            {
                var result = await _bookService.Delete(id);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }
        //Just to check the global exception 
        [HttpGet("globalExceptionTest")]
        public IActionResult ExceptionCheck()
        {
            throw new NotImplementedException();
        }

    }
}
