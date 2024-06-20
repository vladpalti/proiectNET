using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proiect.Data;
using proiect.Models;

namespace proiect.Pages.Movies
{
    [Authorize(Roles = "Admin")]
    public class EditModel : MovieGenresPageModel
    {
        private readonly proiect.Data.proiectContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(proiect.Data.proiectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        public IFormFile CoverArt { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie
               .Include(b => b.Producer)
               .Include(b => b.Director)
               .Include(b => b.MovieGenres).ThenInclude(b => b.Genre)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }
            PopulateAssignedGenreData(_context, Movie);
            ViewData["ProducerID"] = new SelectList(_context.Set<Producer>(), "ID", "ProducerName");
            ViewData["DirectorID"] = new SelectList(_context.Set<Director>(), "ID", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedGenres)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movieToUpdate = await _context.Movie
             .Include(i => i.Producer)
             .Include(i => i.Director)
             .Include(i => i.MovieGenres)
                .ThenInclude(i => i.Genre)
             .FirstOrDefaultAsync(s => s.ID == id);
            if (movieToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Movie>( movieToUpdate,"Movie", i =>  i.Title, i => i.BoxOffice, i => i.Budget, i => i.ReleaseDate, i => i.ProducerID, i => i.DirectorID))
            {
                if (CoverArt != null)
                {
                    string uniqueFileName = UploadFile(CoverArt);
                    movieToUpdate.CoverArtPath = uniqueFileName;
                }
                UpdateMovieGenres(_context, selectedGenres, movieToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

            }
            UpdateMovieGenres(_context, selectedGenres, movieToUpdate);
            PopulateAssignedGenreData(_context, movieToUpdate);
            return Page();
        }
        private string UploadFile(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
