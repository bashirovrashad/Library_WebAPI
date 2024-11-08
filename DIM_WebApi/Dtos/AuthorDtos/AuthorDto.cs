using DIM_WebApi.Dtos.BookDtos;

namespace DIM_WebApi.Dtos.AuthorDtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }


}
