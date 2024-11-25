namespace Authorization.Core.Entities;

public class PasswordEntity
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public UserEntity User { get; set; }
}