namespace DIM_WebApi.Dtos.SaleDtos
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }  // Kitap başlığını da eklemek isteyebilirsiniz
        public int ReaderId { get; set; }
        public string ReaderFullname { get; set; } // Okuyucunun ismi
        public DateTime DateTime { get; set; }
    }
}
