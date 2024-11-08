using DIM_WebApi.Data;
using DIM_WebApi.Dtos;
using DIM_WebApi.Dtos.AuthorDtos;
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
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    Books = a.Books.Select(b => new BookDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Year = b.Year,
                        Price = b.Price
                    }).ToList()
                })
                .ToListAsync();

            return authors;
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .Where(a => a.Id == id)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    Books = a.Books.Select(b => new BookDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Year = b.Year,
                        Price = b.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // POST: api/Author
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            var author = new Author
            {
                Name = createAuthorDto.Name,
                Surname = createAuthorDto.Surname
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, CreateAuthorDto updateAuthorDto)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            author.Name = updateAuthorDto.Name;
            author.Surname = updateAuthorDto.Surname;
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
