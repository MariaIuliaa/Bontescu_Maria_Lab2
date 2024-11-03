using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bontescu_Maria_Lab2.Data;
using Bontescu_Maria_Lab2.Models;

namespace Bontescu_Maria_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public IndexModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;
        public BookData BookD { get; set; }
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        //lab4
        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }
        public string CurrentFilter { get; set; }


        //public async Task OnGetAsync()
        //{
        //    if (_context.Book != null)
        //    {
        //        Book = await _context.Book
        //     .Include(b => b.Author)   // Include și autorul
        //     .Include(b => b.Publisher) // Include și publisher-ul
        //     .Include(b => b.BookCategories) // Include BookCategories
        //      .ThenInclude(bc => bc.Category)
        //     .ToListAsync();

        //    }

        //}
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            BookD = new BookData();
            //lab4
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentFilter = searchString;


            //se va include Author conform cu sarcina de la lab 2
            BookD.Books = await _context.Book
            .Include(b => b.Author)   // Include și 
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                BookD.Books = BookD.Books.Where(s => s.Author.FirstName.Contains(searchString)

               || s.Author.LastName.Contains(searchString)
               || s.Title.Contains(searchString));
            }

            if (id != null)
            {
                BookID = id.Value;
                Book book = BookD.Books
                .Where(i => i.ID == id.Value).Single();
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Title);
                    break;
                case "author_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Author.FullName);
                    break;
                case "author":
                    BookD.Books = BookD.Books.OrderBy(s =>
                   s.Author.FullName);
                    break;
                default:
                    BookD.Books = BookD.Books.OrderBy(s => s.Title);
                    break;
            }
        }
    }
}
