using System;
using System.Collections.Generic;
using System.Linq;


namespace LibraryApp_03._04
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            
            if (context.Authors.Any())
            {
                return;   
            }

            
            var authors = new List<Author>
            {
                new Author { Name = "J.K. Rowling" },
                new Author { Name = "J.R.R. Tolkien" },
                new Author { Name = "George R.R. Martin" }
            };

            context.Authors.AddRange(authors);
            context.SaveChanges();

            
            var books = new List<Book>
            {
                new Book { Title = "Harry Potter and the Sorcerer's Stone", AuthorId = 1 },
                new Book { Title = "The Hobbit", AuthorId = 2 },
                new Book { Title = "A Game of Thrones", AuthorId = 3 }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
