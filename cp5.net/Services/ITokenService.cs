using Cp5.Net.Data;
using Cp5.Net.DTOs;
using Cp5.Net.Models;

namespace Cp5.Net.Services;

public interface ITokenService
{
    Task<User> RegisterAsync(SafeScribeDb db, UserRegisterDto dto, CancellationToken ct);
    Task<LoginResponseDto?> LoginAsync(SafeScribeDb db, LoginRequestDto dto, CancellationToken ct);
}


