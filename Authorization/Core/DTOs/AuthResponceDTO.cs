﻿namespace Authorization.Core.DTOs;

public class AuthResponceDTO
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
}