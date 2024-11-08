namespace DIM_WebApi.Dtos.BookDtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; } 
    }
}
