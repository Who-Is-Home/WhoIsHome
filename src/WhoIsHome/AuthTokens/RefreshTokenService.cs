﻿using Microsoft.EntityFrameworkCore;
using WhoIsHome.DataAccess;
using WhoIsHome.Shared.Exceptions;

namespace WhoIsHome.AuthTokens;

public class RefreshTokenService(WhoIsHomeContext context)
{
    public async Task<RefreshToken> CreateTokenAsync(int userId, CancellationToken cancellationToken)
    {
        var token = RefreshToken.Create(userId);
        var model = token.ToModel();

        var dbToken = await context.RefreshTokens.AddAsync(model, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return dbToken.Entity.ToRefreshToken();
    }

    public async Task<RefreshToken> RefreshAsync(string refreshToken, int userId, CancellationToken cancellationToken)
    {
        var token = await GetValidRefreshToken(refreshToken, userId, cancellationToken);
        var newRefreshToken = token.Refresh();
        
        context.RefreshTokens.Update(token.ToModel());
        await context.RefreshTokens.AddAsync(newRefreshToken.ToModel(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return await CreateTokenAsync(userId, cancellationToken);
    }
    
    private async Task<RefreshToken> GetValidRefreshToken(string tokenToCheck, int userId,
        CancellationToken cancellationToken)
    {
        var model = await context.RefreshTokens
            .SingleOrDefaultAsync(t => t.Token == tokenToCheck, cancellationToken);
        var token = model?.ToRefreshToken();

        if (token is null || token.Validate(userId) is false)
        {
            throw new InvalidRefreshTokenException();
        }

        return token;
    }
}