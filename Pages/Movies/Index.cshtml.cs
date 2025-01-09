using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using proiect.Data;
using proiect.Models;

namespace proiect.Pages.Movies
{
    public class IndexModel : MovieGenresPageModel
    {
        private readonly proiect.Data.proiectContext _context;

        public IndexModel(proiect.Data.proiectContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;
        public MovieData MovieD { get; set; }
        public int MovieID { get; set; }
        public int GenreID { get; set; }
        public string TitleSort { get; set; }
        public string DirectorSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(int? id, int? categoryIDD, string sortOrder, string searchString)
        {
            MovieD = new MovieData();

            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            DirectorSort = sortOrder == "director" ? "director_desc" : "director";

            CurrentFilter = searchString;

            MovieD.Movies = await _context.Movie
            .Include(m => m.Producer)
            .Include(m => m.Director)
            .Include(m => m.MovieGenres)
            .ThenInclude(m => m.Genre)
            .Include(m => m.Reviews)
            .AsNoTracking()
            .OrderBy(m => m.Title)
            .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                MovieD.Movies = MovieD.Movies.Where(s => s.Director.FirstName.Contains(searchString)

               || s.Director.LastName.Contains(searchString)
               || s.Title.Contains(searchString));

                if (id != null)
                {
                    MovieID = id.Value;
                    Movie movie = MovieD.Movies
                    .Where(i => i.ID == id.Value).Single();
                    MovieD.Genres = movie.MovieGenres.Select(s => s.Genre);
                }
                switch (sortOrder)
                {
                    case "title_desc":
                        MovieD.Movies = MovieD.Movies.OrderByDescending(s =>
                       s.Title);
                        break;
                    case "director_desc":
                        MovieD.Movies = MovieD.Movies.OrderByDescending(s =>
                       s.Director.FullName);
                        break;
                    case "director":
                        MovieD.Movies = MovieD.Movies.OrderBy(s =>
                       s.Director.FullName);
                        break;
                    default:
                        MovieD.Movies = MovieD.Movies.OrderBy(s => s.Title);
                        break;
                }
            }
        }
    }
}
