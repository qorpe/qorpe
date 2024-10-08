﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Qorpe.Application.Common.Behaviours;
using System.Reflection;

namespace Qorpe.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        //services.AddSingleton(sp =>
        //{
        //    var env = sp.GetRequiredService<IWebHostEnvironment>();
        //    var path = Path.Combine(env.ContentRootPath, "");
        //    return new FileService(path);
        //});

        return services;
    }
}
