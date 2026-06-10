namespace Trippie.Modules.Auth.Service;

public sealed class OAuthStateEntry
{
    public required string CodeVerifier { get; set; }
    public required DateTime ExpiresAt { get; set; }
}

public sealed class OAuthStateStore
{
    private readonly Dictionary<string, OAuthStateEntry> _states = new();
    private readonly object _lock = new();
    private const int StateExpiryMinutes = 10;

    public void Store(string state, string codeVerifier)
    {
        lock (_lock)
        {
            _states[state] = new OAuthStateEntry
            {
                CodeVerifier = codeVerifier,
                ExpiresAt = DateTime.UtcNow.AddMinutes(StateExpiryMinutes)
            };
        }
    }

    public (bool Found, string? CodeVerifier) TryGet(string state)
    {
        lock (_lock)
        {
            if (!_states.TryGetValue(state, out var entry))
                return (false, null);

            if (entry.ExpiresAt < DateTime.UtcNow)
            {
                _states.Remove(state);
                return (false, null);
            }

            var verifier = entry.CodeVerifier;
            _states.Remove(state);
            return (true, verifier);
        }
    }
}
