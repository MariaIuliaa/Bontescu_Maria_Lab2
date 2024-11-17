using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bontescu_Maria_Lab2.Data;
using Bontescu_Maria_Lab2.Models;
using Microsoft.AspNetCore.Authorization;


namespace Bontescu_Maria_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public CreateModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName");
                ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
                PopulateAssignedCategoryData(_context, Book);
                return Page();
            }

            var newBook = new Book();
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index - Copy");
        }
    }
}
