namespace Cp5.Net.Services;

public interface ITokenBlacklistService
{
    Task AddToBlacklistAsync(string jti, DateTime expiresAtUtc, CancellationToken ct = default);
    Task<bool> IsBlacklistedAsync(string jti, CancellationToken ct = default);
}


