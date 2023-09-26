using BookManagementSystem.Common;
using BookManagementSystem.CommonModels;
using BookManagementSystem.Controllers;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Tests.ControllerTests
{
    
    public class BookControllerTests
    {
        private IBookService _bookService;
        private BooksController _booksController;
        public BookControllerTests()
        {
            _bookService = A.Fake<IBookService>();

            _booksController = new BooksController(_bookService);
        }
        [Fact]
        public async Task BookController_GetBooks_ReturnOK()
        {
            //Arrange
            PageParams page = new PageParams();
            var returnType = A.Fake<ResultModel<Book>>();
            A.CallTo(() => _bookService.GetAll(page)).Returns(returnType);
            var bookList = new ResultModel<Book>();

            //Act
            var result = await _booksController.GetBooks(page.PageNumber, page.PageSize);
            var objectResult = (ObjectResult)result;
            var actual = objectResult.Value;

            var expectedType = typeof(ResultModel<Book>);
            bool isAssignable = expectedType.IsAssignableFrom(actual.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode); 
        }
        [Fact]
        public async Task BookController_GetBook_ReturnOK()
        {
            //Arrange 
            int id = 1;
            var returnType = A.Fake<ResultModel<Book>>();
            A.CallTo(() => _bookService.Get(id)).Returns(returnType);
            var bookList = new ResultModel<List<Book>>();

            //Act
            var result = await _booksController.GetBook(id);
            var objectResult = (ObjectResult)result;
            var actual = objectResult.Value;

            var expectedType = typeof(ResultModel<Book>);
            bool isAssignable = expectedType.IsAssignableFrom(actual.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
        [Fact]
        public async Task BookController_PutBook_ReturnOK()
        {
            //Arrange  
            Book book = new Book();
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookService.Update(book)).Returns(returnType);
            var bookList = new ResultModel<List<Book>>();

            //Act
            var result = await _booksController.PutBook(book);
            var objectResult = (ObjectResult)result;
            var actual = objectResult.Value;

            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(actual.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
        [Fact]
        public async Task BookController_PostBook_ReturnOK()
        {
            //Arrange  
            Book book = new Book();
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookService.Create(book)).Returns(returnType);
            var bookList = new ResultModel<List<Book>>();

            //Act
            var result = await _booksController.PostBook(book);
            var objectResult = (ObjectResult)result;
            var actual = objectResult.Value;

            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(actual.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
        [Fact]
        public async Task BookController_DeleteBook_ReturnOK()
        {
            //Arrange  
            int id = 1;
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookService.Delete(id)).Returns(returnType);
            var bookList = new ResultModel<List<Book>>();

            //Act
            var result = await _booksController.DeleteBook(id);
            var objectResult = (ObjectResult)result;
            var actual = objectResult.Value;

            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(actual.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
    }
}
