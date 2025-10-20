using System.IdentityModel.Tokens.Jwt;
using Cp5.Net.Data;
using Cp5.Net.DTOs;
using Cp5.Net.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cp5.Net.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly SafeScribeDb _db;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _blacklist;

    public AuthController(SafeScribeDb db, ITokenService tokenService, ITokenBlacklistService blacklist)
    {
        _db = db;
        _tokenService = tokenService;
        _blacklist = blacklist;
    }

    [HttpPost("registrar")]
    [AllowAnonymous]
    public async Task<ActionResult> Registrar([FromBody] UserRegisterDto dto, CancellationToken ct)
    {
        try
        {
            var user = await _tokenService.RegisterAsync(_db, dto, ct);
            return CreatedAtAction(nameof(Registrar), new { id = user.Id }, new { user.Id, user.Username, Role = user.Role.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
    {
        var response = await _tokenService.LoginAsync(_db, dto, ct);
        if (response is null)
            return Unauthorized();
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout(CancellationToken ct)
    {
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var exp = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(60);
        if (!string.IsNullOrEmpty(exp) && long.TryParse(exp, out var seconds))
        {
            var epoch = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
            expiresAtUtc = epoch;
        }
        if (!string.IsNullOrEmpty(jti))
        {
            await _blacklist.AddToBlacklistAsync(jti, expiresAtUtc, ct);
        }
        return Ok(new { message = "Logout efetuado" });
    }
}


