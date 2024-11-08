namespace DIM_WebApi.Dtos.BookDtos
{
    public class CreateBookDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
    }
}
