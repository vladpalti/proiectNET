namespace proiect.Models
{
    public class Producer
    {
        public int ID { get; set; }
        public string ProducerName { get; set; }
        public ICollection<Movie>? Movies { get; set; }
    }
}
