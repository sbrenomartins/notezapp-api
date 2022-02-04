using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotezApp.Business.BusinessObjects.Requests;
using NotezApp.Business.Services;
using System.Security.Claims;

namespace NotezApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet("list-all")]
        [Authorize]
        public IActionResult GetNotes([FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = noteService.ListAll(userId);

            if (response == null)
            {
                return BadRequest("Não foi possível recuperar as notas");
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetNoteById(long id, [FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = noteService.ListNoteById(userId, id);

            if (response.NotFound)
            {
                return NotFound(response);
            }

            if (response.Note == null && response.NotFound == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest createNoteRequest,
                                        [FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await noteService.CreateNote(userId, createNoteRequest);

            if (response == null)
                return BadRequest("Ocorreu um erro ao tentar criar a nota");

            return Ok(response);
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNote(long id, [FromBody] CreateNoteRequest createNoteRequest,
                                                    [FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await noteService.UpdateNote(userId, id, createNoteRequest);

            if (response.Forbidden)
            {
                return Unauthorized(response);
            }

            if (response.Note == null && !response.Forbidden)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNote(long id, [FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await noteService.DeleteNote(userId, id);

            if (response.Forbidden)
            {
                return Unauthorized(response);
            }

            if (!response.Forbidden && !response.Deleted)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("delete-all")]
        [Authorize]
        public async Task<IActionResult> DeleteAllNotes([FromServices] NoteService noteService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await noteService.DeleteAllNotes(userId);

            if (response)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
