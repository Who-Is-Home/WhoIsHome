using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WhoIsHome.Aggregates;
using WhoIsHome.Services;
using WhoIsHome.Shared.Authentication;
using WhoIsHome.Shared.Helper;
using WhoIsHome.Shared.Types;
using WhoIsHome.WebApi.Models.New;
using WhoIsHome.WebApi.Models.Response;
using OneTimeEventModel = WhoIsHome.WebApi.Models.Request.OneTimeEventModel;

namespace WhoIsHome.WebApi.AggregatesControllers;

public class OneTimeEventController(
    IOneTimeEventAggregateService oneTimeEventAggregateService, IUserContext userContext)
    : AggregateControllerBase<OneTimeEvent, OneTimeEventModelResponse>(oneTimeEventAggregateService, userContext)
{
    [HttpPost]
    public async Task<ActionResult<OneTimeEventModelResponse>> CreateEvent([FromBody] OneTimeEventModelDto eventModelDto,
        CancellationToken cancellationToken)
    {
        var result = await oneTimeEventAggregateService.CreateAsync(
            title: eventModelDto.Title,
            date: eventModelDto.Date,
            startTime: eventModelDto.StartTime,
            endTime: eventModelDto.EndTime,
            presenceType: PresenceTypeHelper.FromString(eventModelDto.PresenceType),
            time: eventModelDto.DinnerTime,
            cancellationToken: cancellationToken);

        return await BuildResponseAsync(result);
    }

    [HttpPatch]
    public async Task<ActionResult<OneTimeEventModelResponse>> UpdateEvent([FromBody] OneTimeEventModel eventModel,
        CancellationToken cancellationToken)
    {
        var result = await oneTimeEventAggregateService.UpdateAsync(
            id: eventModel.Id,
            title: eventModel.Title,
            date: eventModel.Date,
            startTime: eventModel.StartTime,
            endTime: eventModel.EndTime,
            presenceType: PresenceTypeHelper.FromString(eventModel.PresenceType),
            time: eventModel.DinnerTime,
            cancellationToken: cancellationToken);

        return await BuildResponseAsync(result);
    }

    protected override Task<OneTimeEventModelResponse> ConvertToModelAsync(OneTimeEvent data, User user) =>
        Task.FromResult(OneTimeEventModelResponse.From(data, user));
}