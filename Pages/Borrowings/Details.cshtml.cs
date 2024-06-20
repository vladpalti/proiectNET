using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proiect.Data;
using proiect.Models;

namespace proiect.Pages.Borrowings
{
    public class DetailsModel : PageModel
    {
        private readonly proiect.Data.proiectContext _context;

        public DetailsModel(proiect.Data.proiectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var movieList = _context.Movie
                .Include(b => b.Director)
                .Select(x => new
                {
                    x.ID,
                    MovieFullName = x.Title + " - " + x.Director.LastName + " " + x.Director.FirstName
                });

            ViewData["MovieID"] = new SelectList(movieList, "ID", "MovieFullName");
            ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName");
            return Page();
        }

        public Borrowing Borrowing { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }
            else 
            {
                Borrowing = borrowing;
            }
            return Page();
        }
    }
}
