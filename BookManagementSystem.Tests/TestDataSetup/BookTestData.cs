using BookManagementSystem.Common;
using BookManagementSystem.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Tests.TestDataSetup
{
    public class TestBookList
    {
        public List<Book> books;
    }
    public class BookTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new TestBookList
                {
                    books = new List<Book>()
                    {
                        new Book()
                        {
                            Id = 1,
                            Author = "GRR Martin",
                            Title = "Game of Thrones",
                            Genre = "Sci-Fi",
                            PublicationYear = 2000
                        },
                        new Book()
                        {
                            Id = 2,
                            Author = "Plato",
                            Title = "Alice",
                            Genre = "Sci-Fi",
                            PublicationYear = 2002
                        },
                        new Book()
                        {
                            Id = 3,
                            Author = "John",
                            Title = "Crime and Punishment",
                            Genre = "Crime",
                            PublicationYear = 2020
                        },
                        new Book()
                        {
                            Id = 4,
                            Author = "Doe",
                            Title = "Ikigai",
                            Genre = "Documentry",
                            PublicationYear = 2001
                        }
                    }
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
