using Microsoft.AspNetCore.Mvc;
using WhoIsHome.QueryHandler.DailyOverview;
using WhoIsHome.WebApi.ModelControllers.Models;

namespace WhoIsHome.WebApi.DailyOverviews;

public class DailyOverviewController(DailyOverviewQueryHandler queryHandler) 
    : WhoIsHomeControllerBase<IReadOnlyCollection<DailyOverview>, IReadOnlyCollection<DailyOverviewModel>>
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<DailyOverviewModel>>> GetAsync(CancellationToken cancellationToken)
    {
        var result = await queryHandler.HandleAsync(cancellationToken);
        return BuildResponse(result);
    }

    protected override IReadOnlyCollection<DailyOverviewModel> ConvertToModel(IReadOnlyCollection<DailyOverview> data)
    {
        return data
            .Select(presence => 
                new DailyOverviewModel
                {
                    Person = PersonModel.From(presence.Person), 
                    IsAtHome = presence.IsAtHome, 
                    DinnerAt = presence.DinnerAt?.ToString()
                })
            .ToList();
    }
}