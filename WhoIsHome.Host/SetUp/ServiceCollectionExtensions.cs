﻿using WhoIsHome.DataAccess;
using WhoIsHome.WebApi;

namespace WhoIsHome.Host.SetUp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
        services.AddControllers();
        
        services.AddWhoIsHomeServices(configuration)
            .AddDataAccessServices()
            .AddWebApiServices();
        
        return services;
    }
}