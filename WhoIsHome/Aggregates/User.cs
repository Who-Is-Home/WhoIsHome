﻿using System.Net.Mail;
using WhoIsHome.Shared;

namespace WhoIsHome.Aggregates;

public class User : AggregateBase
{
    private const int UserNameMinLength = 5;
    private const int UserNameMaxLength = 30;

    public int? Id { get; }

    public string UserName { get; private set; }

    public string Email { get; }

    public string PasswordHash { get; private set; }

    private User(int? id, string userName, string email, string passwordHash)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public static User Create(string userName, string email, string passwordHash)
    {
        // validates Email, throws if Invalid format
        _ = new MailAddress(email);
        
        if (IsValidUserName(userName))
            throw new ArgumentException(
                $"UserName is to long or to short. Must be between {UserNameMinLength} and {UserNameMaxLength} Characters.",
                nameof(userName));
        
        return new User(
            null,
            userName,
            email,
            passwordHash);
    }

    private static bool IsValidUserName(string userName)
    {
        return userName.Length is > UserNameMaxLength or < UserNameMinLength;
    }
}