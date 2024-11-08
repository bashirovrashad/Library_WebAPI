namespace DIM_WebApi.Dtos.SaleDtos
{
    public class CreateSaleDto
    {
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
