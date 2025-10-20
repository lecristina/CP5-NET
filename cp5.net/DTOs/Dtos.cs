using Cp5.Net.Models;

namespace Cp5.Net.DTOs;

public record UserRegisterDto(string Username, string Password, Role Role);
public record LoginRequestDto(string Username, string Password);
public record LoginResponseDto(string Token, DateTime ExpiresAtUtc);
public record NoteCreateDto(string Title, string Content);
public record NoteUpdateDto(string Title, string Content);


