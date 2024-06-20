﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using proiect.Data;
using proiect.Models;

namespace proiect.Pages.Genres
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly proiect.Data.proiectContext _context;

        public DeleteModel(proiect.Data.proiectContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Genre Genre { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var category = await _context.Genre.FirstOrDefaultAsync(m => m.ID == id);

            if (category == null)
            {
                return NotFound();
            }
            else 
            {
                Genre = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }
            var category = await _context.Genre.FindAsync(id);

            if (category != null)
            {
                Genre = category;
                _context.Genre.Remove(Genre);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
