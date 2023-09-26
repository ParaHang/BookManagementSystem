using BookManagementSystem.Common;
using BookManagementSystem.CommonModels;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Repository.Interfaces;
using BookManagementSystem.Services;
using BookManagementSystem.Tests.TestDataSetup;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace BookManagementSystem.Tests.ServiceTests
{
    public class BookServiceTests
    {
        private IBookService _bookService;
        private readonly IBookRepository _bookRepository;
        public BookServiceTests()
        {
            _bookRepository = A.Fake<IBookRepository>();

            _bookService = new BookService(_bookRepository);
        }

        [Theory]
        [ClassData(typeof(BookTestData))]
        public async Task BookService_GetAll_ReturnBooks(TestBookList testData)
        {
            //Arrange
            ResultModel<Book> resultModel = new ResultModel<Book>()
            {
                status = "00",
                message = "Operation Successful",
                success = true,
                data = (testData.books)
            };
            PageParams page = new PageParams();
            var booklist = A.Fake<ResultModel<Book>>();
            A.CallTo(() => _bookRepository.GetAll(page)).Returns(resultModel);

            //Act
            ResultModel<Book> result = await _bookService.GetAll(page);
            var expectedType = typeof(ResultModel<Book>);
            bool isAssignable = expectedType.IsAssignableFrom(result.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.NotNull(result.data);
            Assert.Equal(resultModel?.data, result.data);
        }
        [Theory]
        [ClassData(typeof(BookTestData))]
        public async Task BookService_Get_ReturnBook(TestBookList testData)
        {
            //Arrange
            int id = 1;
            var book = A.Fake<ResultModel<Book>>();
            ResultModel<Book> resultModel = new ResultModel<Book>()
            {
                status = "00",
                message = "Operation Successful",
                success = true
            };
            resultModel.data.Add(testData.books.FirstOrDefault(x=>x.Id == id));

            A.CallTo(() => _bookRepository.Get(id)).Returns(resultModel);

            //Act
            ResultModel<Book> result = await _bookService.Get(id);
            var expectedType = typeof(ResultModel<Book>);
            bool isAssignable = expectedType.IsAssignableFrom(result.GetType());

            //Assert

            Assert.True(isAssignable);
            Assert.NotNull(result);
            Assert.Equal(id, result.data[0].Id);

        }

        [Fact]
        public async Task BookService_Create_ReturnSuccess()
        {
            //Arrange
            Book book = new Book();
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookRepository.Create(book)).Returns(returnType);

            //Act
            var result = await _bookService.Create(book);
            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(result.GetType());

            //Assert

            Assert.True(isAssignable);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task BookService_Delete_ReturnSuccess()
        {
            //Arrange
            int id = 1;
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookRepository.Delete(id)).Returns(returnType);

            //Act
            var result = await _bookService.Delete(id);
            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(result.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task BookService_PutBook_ReturnBook()
        {
            //Arrange
            Book book = new Book();
            var returnType = A.Fake<ResultModel<string>>();
            A.CallTo(() => _bookRepository.Update(book)).Returns(returnType);

            //Act
            var result = await _bookService.Update(book);
            var expectedType = typeof(ResultModel<string>);
            bool isAssignable = expectedType.IsAssignableFrom(result.GetType());

            //Assert
            Assert.True(isAssignable);
            Assert.NotNull(result);
        }
    }
}
