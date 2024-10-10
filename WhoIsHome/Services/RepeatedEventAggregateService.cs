﻿using Microsoft.EntityFrameworkCore;
using WhoIsHome.Aggregates;
using WhoIsHome.Aggregates.Mappers;
using WhoIsHome.DataAccess;
using WhoIsHome.DataAccess.Models;
using WhoIsHome.Shared.Authentication;
using WhoIsHome.Shared.Exceptions;
using WhoIsHome.Shared.Types;

namespace WhoIsHome.Services;

public class RepeatedEventAggregateService(WhoIsHomeContext context, IUserContext userContext) : IAggregateService<RepeatedEvent>
{
    public async Task<RepeatedEvent> GetAsync(int id, CancellationToken cancellationToken)
    {
        var result = await context.RepeatedEvents
            .Include(e => e.UserModel)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (result is null) throw new NotFoundException($"No RepeatedEvent found with the id {id}.");

        return result.ToAggregate();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var result = await context.RepeatedEvents
            .Include(repeatedEventModel => repeatedEventModel.UserModel)
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (result is null) throw new NotFoundException($"No RepeatedEvent found with the id {id}.");

        if (!userContext.IsUserPermitted(result.UserModel.Id))
        {
            throw new ActionNotAllowedException($"User with ID {result.UserModel.Id} is not allowed to delete or modify the content of {id}");
        }
        
        context.RepeatedEvents.Remove(result);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RepeatedEvent> CreateAsync(string title, DateOnly firstOccurrence, DateOnly lastOccurrence,
        TimeOnly startTime, TimeOnly endTime, PresenceType presenceType, TimeOnly? time, CancellationToken cancellationToken)
    {
        var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId, cancellationToken: cancellationToken);
        var repeatedEvent = RepeatedEvent
            .Create(title, firstOccurrence, lastOccurrence, startTime, endTime, presenceType, time, user.Id)
            .ToModel(user);

        var result = await context.RepeatedEvents.AddAsync(repeatedEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return result.Entity.ToAggregate();
    }

    public async Task<RepeatedEvent> UpdateAsync(int id, string title, DateOnly firstOccurrence,
        DateOnly lastOccurrence, TimeOnly startTime, TimeOnly endTime, PresenceType presenceType, TimeOnly? time,
        CancellationToken cancellationToken)
    {
        var aggregate = await GetAsync(id, cancellationToken);
        var user = await context.Users.SingleAsync(u => u.Id == aggregate.UserId, cancellationToken: cancellationToken);
        
        if (!userContext.IsUserPermitted(user.Id))
        {
            throw new ActionNotAllowedException($"User with ID {user.Id} is not allowed to delete or modify the content of {id}");
        }

        aggregate.Update(title, firstOccurrence, lastOccurrence, startTime, endTime, presenceType, time);
        
        var result = context.RepeatedEvents.Update(aggregate.ToModel(user));
        await context.SaveChangesAsync(cancellationToken);
        return result.Entity.ToAggregate();
    }
}