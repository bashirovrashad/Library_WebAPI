using DIM_WebApi.Dtos.BookDtos;

namespace DIM_WebApi.Dtos.ReaderDtos
{
    public class ReaderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<BookDto> Books { get; set; } // Kitapları ilişkilendirmek için
    }
}
