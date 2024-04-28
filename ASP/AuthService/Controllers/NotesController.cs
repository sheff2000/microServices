using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AuthService.Data;
using AuthService.Models;
using MongoDB.Driver;
using System.Linq;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;
        public NotesController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // все записи
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _dbContext.Notes.Find(_ => true).ToListAsync();
            return Ok(notes);
        }

        // заметки только текущего юзера
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserNotes()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var notes = await _dbContext.Notes.Find(note => note.UserId == userId).ToListAsync();
            return Ok(notes);
        }

        // детально заметку по айди
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(string id)
        {
            var note = await _dbContext.Notes.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (note == null)
                return NotFound("Note not found.");
            return Ok(note);
        }

        // создание заметки
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            note.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;
            await _dbContext.Notes.InsertOneAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        // редактирование заметки
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNote(string id, [FromBody] Note updatedNote)
        {
            var note = await _dbContext.Notes.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (note == null)
                return NotFound("Note not found.");

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (note.UserId != userId && HttpContext.User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid("You are not authorized to update this note.");

            updatedNote.UpdatedAt = DateTime.UtcNow;
            await _dbContext.Notes.ReplaceOneAsync(n => n.Id == id, updatedNote);
            return NoContent();
        }

        // удаление заметки владелец и админ
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNote(string id)
        {
            var note = await _dbContext.Notes.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (note == null)
                return NotFound("Note not found.");

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (note.UserId != userId && HttpContext.User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid("You are not authorized to delete this note.");

            await _dbContext.Notes.DeleteOneAsync(n => n.Id == id);
            return NoContent();
        }


    }
}
