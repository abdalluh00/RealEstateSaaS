using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Jobs;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.Services;
using RealEstate.Infrastructure.Settings;
using RealEstate.Shared.Settings;

namespace RealEstate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                ));

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IPropertyMediaRepository, PropertyMediaRepository>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<ILandRepository, LandRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IVillaRepository, VillaRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Services
            services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
            services.AddScoped<IWhatsAppService, WhatsAppService>();

            // Hangfire
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();

            // Background Jobs
            services.AddScoped<INotificationJobService, NotificationJobService>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<ITenantService, TenantService>();

            return services;
        }
    }
}
