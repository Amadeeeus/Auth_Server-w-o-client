namespace Authorization.Core.JWT;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public int TokenLifetimeMinutes { get; set; } = 30;
}