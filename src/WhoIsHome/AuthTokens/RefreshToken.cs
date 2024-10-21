﻿using System.Security.Cryptography;

namespace WhoIsHome.AuthTokens;

public class RefreshToken(int? id, int userId, string token, DateTime issued, DateTime? expiredAt)
{
    private const int ExpiresInDays = 12;

    private const int RefreshTokenLength = 64;

    public int? Id { get; private init; } = id;

    public int UserId { get; private init; } = userId;
    
    public string Token { get; private init; } = token;

    public DateTime Issued { get; private set; } = issued;

    public DateTime? ExpiredAt { get; set; } = expiredAt;

    public bool Validate(int requestUser)
    {
        if (requestUser != UserId)
        {
            return false;
        }
        
        if (ExpiredAt.HasValue)
        {
            return false;
        }

        if (Issued.AddDays(ExpiresInDays) < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public static RefreshToken Create(int userId)
    {
        var token = GenerateToken();
        return new RefreshToken(null, userId, token, DateTime.Now, null);
    }

    public RefreshToken Refresh()
    {
        ExpiredAt = DateTime.Now;
        var token = GenerateToken();
        return new RefreshToken(null, UserId, token, DateTime.Now, null);
    }
    
    private static string GenerateToken()
    {
        var randomNumber = new byte[RefreshTokenLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}