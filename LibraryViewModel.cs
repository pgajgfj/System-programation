using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraryApp_03._04
{
    public class LibraryViewModel
    {
        private LibraryContext _context;

        public ObservableCollection<Book> Books { get; set; }

        public LibraryViewModel(LibraryContext context)
        {
            _context = context;
            Books = new ObservableCollection<Book>();
            LoadBooks();
        }

       
        public void LoadBooks()
        {
            var books = _context.Books.Include(b => b.Author).ToList();
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        
        public void FilterBooksByAuthor(Author author)
        {
            var filteredBooks = _context.Books
                                         .Where(b => b.Author.Id == author.Id)
                                         .ToList();
            Books.Clear();
            foreach (var book in filteredBooks)
            {
                Books.Add(book);
            }
        }

        
        public void FilterBooksBySearch(string searchText)
        {
            var filteredBooks = _context.Books
                                         .Where(b => b.Title.Contains(searchText) || b.Author.Name.Contains(searchText))
                                         .ToList();
            Books.Clear();
            foreach (var book in filteredBooks)
            {
                Books.Add(book);
            }
        }
    }
}
