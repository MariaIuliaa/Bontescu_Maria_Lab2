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

namespace Bontescu_Maria_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context _context;

        public EditModel(Bontescu_Maria_Lab2.Data.Bontescu_Maria_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (borrowing == null)
            {
                return NotFound();
            }
            Borrowing = borrowing;

            // Update SelectList to display Book Titles and Member Names instead of IDs
            ViewData["BookID"] = new SelectList(_context.Book, "ID", "Title");
            ViewData["MemberID"] = new SelectList(_context.Member.Select(m => new {
                ID = m.ID,
                FullName = m.FirstName + " " + m.LastName
            }), "ID", "FullName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return (_context.Borrowing?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
