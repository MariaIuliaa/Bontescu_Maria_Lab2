using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bontescu_Maria_Lab2.Data;
using Bontescu_Maria_Lab2.Models;
using Bontescu_Maria_Lab2.ViewModels; // Include the ViewModel namespace

namespace Bontescu_Maria_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public IndexModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;
        }

        public CategoryBooksViewModel CategoryBooks { get; set; } = new CategoryBooksViewModel();

        public async Task OnGetAsync(int? categoryId)
        {
            CategoryBooks.Categories = await _context.Category.ToListAsync();

            if (categoryId.HasValue)
            {
                CategoryBooks.Books = await _context.Book
                    .Include(b => b.Author)
                    .Where(b => b.BookCategories.Any(bc => bc.CategoryID == categoryId))
                    .ToListAsync();
            }
            else
            {
                CategoryBooks.Books = new List<Book>(); // Asigură că lista nu este niciodată null
            }
        }
    }
}
