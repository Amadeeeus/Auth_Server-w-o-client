namespace Authorization.Core.Entities;

public class UserEntity
{
    public string Id{ get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; } = DateTime.UtcNow.AddDays(10);
    public PasswordEntity PasswordEntity { get; set; }
}