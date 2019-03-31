using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SchoolDatabase.UWP.DataAccess;
using SchoolDatabase.UWP.Model;

namespace SchoolDatabase.UWP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoursesController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentCoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourses
        [HttpGet]
        public IEnumerable<StudentCourse> GetStudentCourses()
        {
            return _context.StudentCourses;
        }

        // GET: api/StudentCourses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCourse = await _context.StudentCourses.FindAsync(id);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return Ok(studentCourse);
        }

        // PUT: api/StudentCourses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse([FromRoute] int id, [FromBody] StudentCourse studentCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentCourse.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(studentCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(id))
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

        // POST: api/StudentCourses
        [HttpPost]
        public async Task<IActionResult> PostStudentCourse([FromBody] StudentCourse studentCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentCourses.Add(studentCourse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentCourseExists(studentCourse.StudentId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentCourse", new { id = studentCourse.StudentId }, studentCourse);
        }

        // DELETE: api/StudentCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCourse = await _context.StudentCourses.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return Ok(studentCourse);
        }

        private bool StudentCourseExists(int id)
        {
            return _context.StudentCourses.Any(e => e.StudentId == id);
        }
    }
}
