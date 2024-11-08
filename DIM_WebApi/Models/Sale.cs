namespace DIM_WebApi.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader { get; set; }

        public DateTime DateTime { get; set; }
    }
}
