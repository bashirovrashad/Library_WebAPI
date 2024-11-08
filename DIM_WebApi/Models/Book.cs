namespace DIM_WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Year { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
