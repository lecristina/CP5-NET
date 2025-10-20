using System.Security.Claims;
using Cp5.Net.Data;
using Cp5.Net.DTOs;
using Cp5.Net.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cp5.Net.Controllers;

[ApiController]
[Route("api/v1/notas")]
[Authorize]
public class NotasController : ControllerBase
{
    private readonly SafeScribeDb _db;
    public NotasController(SafeScribeDb db)
    {
        _db = db;
    }

    [HttpPost]
    [Authorize(Roles = "Editor,Admin")]
    public async Task<ActionResult> Criar([FromBody] NoteCreateDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            UserId = Guid.Parse(userId)
        };
        _db.Notes.Add(note);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Obter), new { id = note.Id }, note);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> Obter([FromRoute] Guid id, CancellationToken ct)
    {
        var note = await _db.Notes.FindAsync(new object[] { id }, ct);
        if (note is null) return NotFound();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (role != Role.Admin.ToString())
        {
            if (note.UserId.ToString() != userId)
                return Forbid();
        }
        return Ok(note);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromBody] NoteUpdateDto dto, CancellationToken ct)
    {
        var note = await _db.Notes.FindAsync(new object[] { id }, ct);
        if (note is null) return NotFound();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (role != Role.Admin.ToString() && note.UserId.ToString() != userId)
            return Forbid();

        note.Title = dto.Title;
        note.Content = dto.Content;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Apagar([FromRoute] Guid id, CancellationToken ct)
    {
        var note = await _db.Notes.FindAsync(new object[] { id }, ct);
        if (note is null) return NotFound();
        _db.Notes.Remove(note);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}


