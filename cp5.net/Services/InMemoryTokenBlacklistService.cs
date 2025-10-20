namespace Cp5.Net.Services;

public class InMemoryTokenBlacklistService : ITokenBlacklistService
{
    private readonly Dictionary<string, DateTime> _blacklist = new();

    public Task AddToBlacklistAsync(string jti, DateTime expiresAtUtc, CancellationToken ct = default)
    {
        _blacklist[jti] = expiresAtUtc;
        return Task.CompletedTask;
    }

    public Task<bool> IsBlacklistedAsync(string jti, CancellationToken ct = default)
    {
        if (_blacklist.TryGetValue(jti, out var expiresAt) && expiresAt > DateTime.UtcNow)
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}


