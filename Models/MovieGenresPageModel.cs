using Microsoft.AspNetCore.Mvc.RazorPages;
using proiect.Data;

namespace proiect.Models
{
    public class MovieGenresPageModel : PageModel
    {
        public List<AssignedGenreData> AssignedGenreDataList;
        public void PopulateAssignedGenreData(proiectContext context,
        Movie movie)
        {
            var allGenres = context.Genre;
            var movieGenres = new HashSet<int>(
            movie.MovieGenres.Select(c => c.GenreID)); 
            AssignedGenreDataList = new List<AssignedGenreData>();
            foreach (var gen in allGenres)
            {
                AssignedGenreDataList.Add(new AssignedGenreData
                {
                    GenreID = gen.ID,
                    Name = gen.GenreName,
                    Assigned = movieGenres.Contains(gen.ID)
                });
            }
        }
        public void UpdateMovieGenres(proiectContext context,
         string[] selectedGenres, Movie movieToUpdate)
        {
            if (selectedGenres == null)
            {
                movieToUpdate.MovieGenres = new List<MovieGenre>();
                return;
            }
            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var movieGenres = new HashSet<int>
            (movieToUpdate.MovieGenres.Select(c => c.Genre.ID));
            foreach (var gen in context.Genre)
            {
                if (selectedGenresHS.Contains(gen.ID.ToString()))
                {
                    if (!movieGenres.Contains(gen.ID))
                    {
                        movieToUpdate.MovieGenres.Add(
                        new MovieGenre
                        {
                            MovieID = movieToUpdate.ID,
                            GenreID = gen.ID
                        });
                    }
                }
                else
                {
                    if (movieGenres.Contains(gen.ID))
                    {
                        MovieGenre courseToRemove
                        = movieToUpdate
                        .MovieGenres

                        .SingleOrDefault(i => i.GenreID == gen.ID);
                        context.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}
