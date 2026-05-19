namespace Trippie.Common.Services.Authentication.Jwt;

public interface ITokenBlocklistService
{
    void Revoke(string jti, DateTimeOffset expiry);
    bool IsRevoked(string jti);
}
