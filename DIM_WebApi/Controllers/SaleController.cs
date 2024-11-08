using DIM_WebApi.Data;
using DIM_WebApi.Dtos.SaleDtos;
using DIM_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIM_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
        {
            var sales = await _context.Sales
                .Include(s => s.Book)
                .Include(s => s.Reader)
                .Select(s => new SaleDto
                {
                    Id = s.Id,
                    BookId = s.BookId,
                    BookName = s.Book.Name,
                    ReaderId = s.ReaderId,
                    ReaderFullname = s.Reader.Name+" "+s.Reader.Surname,
                    DateTime = s.DateTime
                })
                .ToListAsync();

            return Ok(sales);
        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Book)
                .Include(s => s.Reader)
                .Where(s => s.Id == id)
                .Select(s => new SaleDto
                {
                    Id = s.Id,
                    BookId = s.BookId,
                    BookName = s.Book.Name,
                    ReaderId = s.ReaderId,
                    ReaderFullname = s.Reader.Name + " " + s.Reader.Surname,
                    DateTime = s.DateTime
                })
                .FirstOrDefaultAsync();

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        // POST: api/Sale
        [HttpPost]
        public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto createSaleDto)
        {
            var sale = new Sale
            {
                BookId = createSaleDto.BookId,
                ReaderId = createSaleDto.ReaderId,
                DateTime = createSaleDto.DateTime
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var saleDto = new SaleDto
            {
                Id = sale.Id,
                BookId = sale.BookId,
                BookName = (await _context.Books.FindAsync(sale.BookId))?.Name,
                ReaderId = sale.ReaderId,
                ReaderFullname = (await _context.Readers.FindAsync(sale.ReaderId))?.Name + (await _context.Readers.FindAsync(sale.ReaderId))?.Surname,
                DateTime = sale.DateTime
            };

            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, saleDto);
        }

        // PUT: api/Sale/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int id, CreateSaleDto updateSaleDto)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            sale.BookId = updateSaleDto.BookId;
            sale.ReaderId = updateSaleDto.ReaderId;
            sale.DateTime = updateSaleDto.DateTime;

            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
