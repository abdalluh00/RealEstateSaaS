using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Extensions;
using RealEstate.Infrastructure.Jobs;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.Repositories.Properties;
using RealEstate.Infrastructure.Services;
using RealEstate.Infrastructure.Settings;
using RealEstate.Shared.Settings;
using Serilog;

namespace RealEstate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── DbContext ─────────────────────────────────
            services.AddDbContext<AppDbContext>(options =>
     options
         .UseSqlServer(
             configuration.GetConnectionString("DefaultConnection"),
             b => b.MigrationsAssembly(
                 typeof(AppDbContext).Assembly.FullName))
         .LogTo(
             message => Log.Debug(message),
             LogLevel.Information)
         .EnableSensitiveDataLogging(
             Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
             == "Development"));

            // ── UnitOfWork ────────────────────────────────
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Property Repositories ─────────────────────
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IVillaRepository, VillaRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<ILandRepository, LandRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();

            // ── Property Files ────────────────────────────
            services.AddScoped<IPropertyMediaRepository, PropertyMediaRepository>();
            services.AddScoped<IPropertyDocumentRepository, PropertyDocumentRepository>();

            // ── Core Repositories ─────────────────────────
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IChequeRepository, ChequeRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            // ── Maintenance ───────────────────────────────
            services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
            services.AddScoped<IMaintenanceMediaRepository, MaintenanceMediaRepository>();

            // ── Code Generators ───────────────────────────
            services.AddScoped<IPropertyCodeGenerator, PropertyCodeGenerator>();
            services.AddScoped<IContractNumberGenerator, ContractNumberGenerator>();
            services.AddScoped<IMaintenanceNumberGenerator, MaintenanceNumberGenerator>();

            // ── Services ──────────────────────────────────
            services.AddScoped<IPaymentGeneratorService, PaymentGeneratorService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IWhatsAppService, WhatsAppService>();

            // ── Auth & Tenant ─────────────────────────────
            services.AddHttpContextAccessor();
          //  services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<ITenantService, TenantService>();

            // ── Settings ──────────────────────────────────
            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));
            services.Configure<TwilioSettings>(
                configuration.GetSection("Twilio"));

            // ── Hangfire ──────────────────────────────────
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();
            services.AddScoped<INotificationJobService, NotificationJobService>();

            // ── Rate Limiting ─────────────────────────────
            services.AddRateLimiting(configuration);

            return services;
        }
    }
}