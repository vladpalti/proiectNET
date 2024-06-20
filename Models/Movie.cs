using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Xml.Linq;

namespace proiect.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Display(Name = "Movie Title")]
        [Required(ErrorMessage = "The Movie Title field is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The Movie Title must be between 3 and 150 characters.")]
        public string Title { get; set; }

        [Column(TypeName = "decimal(6, 2)")][Range(0.01, 1000)]
        public decimal BoxOffice { get; set; }
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public int? ProducerID { get; set; }
        public Producer? Producer { get; set; }
        public int? DirectorID { get; set; }
        public Director? Director { get; set; }
        public int? BorrowingID { get; set; }
        public Borrowing? Borrowing { get; set; }
        public ICollection<MovieGenre>? MovieGenres { get; set; }
        [Display(Name = "Cover Art")]
        public string? CoverArtPath { get; set; }
        public bool Success
        {
            get
            {
                decimal revenuePercentage = (BoxOffice - Budget) / Budget;
                return revenuePercentage >= 0.2m; 
            }
        }
    }
}
