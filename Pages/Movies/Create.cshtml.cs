using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using proiect.Models;
using System.Data;

namespace proiect.Pages.Movies
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : MovieGenresPageModel
    {
        private readonly proiect.Data.proiectContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModel(proiect.Data.proiectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult OnGet()
        {
            ViewData["ProducerID"] = new SelectList(_context.Set<Producer>(), "ID", "ProducerName");
            ViewData["DirectorID"] = new SelectList(_context.Set<Director>(), "ID", "FullName");
            var movie = new Movie();
            movie.MovieGenres = new List<MovieGenre>();
            PopulateAssignedGenreData(_context, movie);
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        public IFormFile CoverArt { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedGenres)
        {
            var newMovie = new Movie();
            if (selectedGenres != null)
            {
                newMovie.MovieGenres = new List<MovieGenre>();
                foreach (var gen in selectedGenres)
                {
                    var catToAdd = new MovieGenre
                    {
                        GenreID = int.Parse(gen)
                    };
                    newMovie.MovieGenres.Add(catToAdd);
                }
            }
            Movie.MovieGenres = newMovie.MovieGenres;
            if (CoverArt != null && CoverArt.Length > 0)
            {
                string uniqueFileName = UploadFile(CoverArt);
                Movie.CoverArtPath = uniqueFileName;
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
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
