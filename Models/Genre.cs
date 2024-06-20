namespace proiect.Models
{
    public class Genre
    {
        public int ID { get; set; }
        public string GenreName { get; set; }
        public ICollection<MovieGenre>? MovieGenres { get; set; }
    }
}
