using System.ComponentModel.DataAnnotations;
using static System.Reflection.Metadata.BlobBuilder;

namespace proiect.Models
{
    public class Borrowing
    {
        public int ID { get; set; }
        public int? MemberID { get; set; }
        public Member? Member { get; set; }
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }
        [DataType(DataType.Date)] public DateTime ReturnDate { get; set; }
    }
}
