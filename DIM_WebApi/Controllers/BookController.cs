using DIM_WebApi.Data;
using DIM_WebApi.Dtos;
using DIM_WebApi.Dtos.BookDtos;
using DIM_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIM_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name
                })
                .ToListAsync();

            return books;
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            var book = new Book
            {
                Name = createBookDto.Name,
                Year = createBookDto.Year,
                Price = createBookDto.Price,
                AuthorId = createBookDto.AuthorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Yeni eklenen kitabın DTO'sunu döndür
            var bookDto = new BookDto
            {
                Id = book.Id,
                Name = book.Name,
                Year = book.Year,
                Price = book.Price,
                AuthorName = (await _context.Authors.FindAsync(book.AuthorId))?.Name
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, CreateBookDto updateBookDto)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.Name = updateBookDto.Name;
            book.Year = updateBookDto.Year;
            book.Price = updateBookDto.Price;
            book.AuthorId = updateBookDto.AuthorId;

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
