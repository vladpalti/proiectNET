namespace proiect.Models
{
    public class MovieData
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public int? MovieID { get; set; }
        public int? GenreID { get; set; }
    }

}
