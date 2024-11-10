using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bontescu_Maria_Lab2.Data;
using Bontescu_Maria_Lab2.Models;

namespace Bontescu_Maria_Lab2.Pages.Borrowings
{
    public class DetailsModel : PageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public DetailsModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;
        }

      public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing
                //lab5
                .Include(b => b.Member)  // Includem membru
                .Include(b => b.Book)    // Includem cartea
                .FirstOrDefaultAsync(m => m.ID == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            Borrowing = borrowing;
            return Page();
        }

    }
}
