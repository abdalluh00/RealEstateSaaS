using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Common.Behaviors;
using System.Reflection;
namespace RealEstate.Application
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TenantBehavior<,>));
            });

             services.AddHttpContextAccessor(); // ← مهم

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMapster();

            return services;
        }
    }
}
