namespace LetsMeet.Infrastructure.Options;

internal class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public TimeSpan Expiry { get; set; }
}