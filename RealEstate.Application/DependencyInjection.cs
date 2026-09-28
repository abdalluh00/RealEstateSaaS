using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Common.Behaviors;
using System.Reflection;

namespace RealEstate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // ── MediatR + Pipeline ────────────────────────
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                // Order: Auth → Tenant → Validation → Handler
                cfg.AddBehavior(
                    typeof(IPipelineBehavior<,>),
                    typeof(AuthorizationBehavior<,>));

                cfg.AddBehavior(
                    typeof(IPipelineBehavior<,>),
                    typeof(TenantBehavior<,>));

                cfg.AddBehavior(
                    typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehavior<,>));
            });

            // ── FluentValidation ──────────────────────────
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}