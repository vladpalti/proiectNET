using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proiect.Models
{
    public class Review
    {
        public int ID { get; set; }

        [Required] 
        public string UserId { get; set; }

        [BindNever]
        public string User { get; set; } 

        [Required]
        public int MovieID { get; set; }
        [BindNever]
        public string Movie { get; set; } 

        [Required]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }
    }

}
