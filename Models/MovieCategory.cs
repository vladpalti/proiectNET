﻿namespace proiect.Models
{
    public class MovieGenre
    {
        public int ID { get; set; }
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
        public int GenreID { get; set; }
        public Genre Genre { get; set; }
    }
}
