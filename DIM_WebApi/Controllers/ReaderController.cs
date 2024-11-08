using DIM_WebApi.Data;
using DIM_WebApi.Dtos.BookDtos;
using DIM_WebApi.Dtos.ReaderDtos;
using DIM_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIM_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReaderController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Reader
       [HttpGet]
        public async Task<ActionResult<IEnumerable<ReaderDto>>> GetReaders()
        {
            var readers = await _context.Readers.Select(r => new ReaderDto
            {
                Id = r.Id,
                Name = r.Name,
                Surname = r.Surname

            }).ToListAsync();
            return readers;
        }



        // GET: api/Reader/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReaderDto>> GetReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }

            return new ReaderDto
            {
                Id = reader.Id,
                Name = reader.Name,
                Surname = reader.Surname
            };
        }







        // POST: api/Reader
        [HttpPost]
        public async Task<ActionResult<ReaderDto>> CreateReader(CreateReaderDto createReaderDto)
        {
            var reader = new Reader
            {
                Name = createReaderDto.Name,
                Surname = createReaderDto.Surname
            };

            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            var readerDto = new ReaderDto
            {
                Id = reader.Id,
                Name = reader.Name,
                Surname = reader.Surname
            };

            return CreatedAtAction(nameof(GetReader), new { id = reader.Id }, readerDto);
        }

        // PUT: api/Reader/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReader(int id, CreateReaderDto createReaderDto)
        {
            if (id != createReaderDto.Id)
            {
                return BadRequest();
            }

            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }

            reader.Name = createReaderDto.Name;
            reader.Surname = createReaderDto.Surname;

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
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

        // DELETE: api/Reader/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReaderExists(int id)
        {
            return _context.Readers.Any(e => e.Id == id);
        }






    }
}
