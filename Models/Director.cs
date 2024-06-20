namespace proiect.Models
{
    public class Director
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ICollection<Movie>? Movies { get; set; }
    }
}
