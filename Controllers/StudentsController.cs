using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Models;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // makes URL
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Bring DB into controller
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET (all)
        [HttpGet]
public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
    [FromQuery] string? search, 
    [FromQuery] int pageNumber = 1, 
    [FromQuery] int pageSize = 10)
{
    // Base query
    var query = _context.Students.AsQueryable();

    // Filter
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search));
    }

    // Pagin.
    var students = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(students);
}
        // GET (one)
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return NotFound();

            return student;
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Returns a 201 and id
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id) return BadRequest();

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
