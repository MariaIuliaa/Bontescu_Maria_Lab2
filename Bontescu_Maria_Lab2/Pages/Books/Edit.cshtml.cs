using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bontescu_Maria_Lab2.Data;
using Bontescu_Maria_Lab2.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Bontescu_Maria_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public EditModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;

        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        // Proprietate pentru categoriile alocate
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } = new List<AssignedCategoryData>();


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            Book = await _context.Book
     .Include(b => b.Author)
     .Include(b => b.Publisher)
     .Include(b => b.BookCategories) // Include relația BookCategories
     .ThenInclude(bc => bc.Category)  // Include și Categoria asociată
     .FirstOrDefaultAsync(m => m.ID == id);



            if (Book == null)
            {
                return NotFound();
            }

            // Populează ViewData pentru Author și Publisher
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            });
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            // Populează AssignedCategoryDataList pentru a putea afișa categoriile selectate
            PopulateAssignedCategoryData(_context, Book);

            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                b => b.Title, b => b.Price, b => b.PublishingDate, b => b.AuthorID, b => b.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);

                try
                {
                    Debug.WriteLine("Attempting to save changes to the database...");
                    await _context.SaveChangesAsync();
                    Debug.WriteLine("Changes successfully saved to the database.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(bookToUpdate.ID))
                    {
                        Debug.WriteLine("Book no longer exists, returning NotFound.");
                        return NotFound();
                    }
                    else
                    {
                        Debug.WriteLine("Concurrency exception occurred.");
                        throw;
                    }
                }

                return RedirectToPage("./Index - Copy");
            }

            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }


        private void UpdateBookCategories(Bontescu_Maria_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            Debug.WriteLine("Entering UpdateBookCategories method");

            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>(bookToUpdate.BookCategories.Select(c => c.CategoryID));

            foreach (var category in context.Category)
            {
                if (selectedCategoriesHS.Contains(category.ID.ToString()))
                {
                    if (!bookCategories.Contains(category.ID))
                    {
                        bookToUpdate.BookCategories.Add(new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = category.ID
                        });
                    }
                }
                else
                {
                    if (bookCategories.Contains(category.ID))
                    {
                        var categoryToRemove = bookToUpdate.BookCategories.FirstOrDefault(c => c.CategoryID == category.ID);
                        if (categoryToRemove != null)
                        {
                            context.Remove(categoryToRemove);
                        }
                    }
                }
            }
        }

        private bool BookExists(int id)
        {
            return (_context.Book?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
