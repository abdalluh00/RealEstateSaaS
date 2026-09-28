using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.API.Authorization;
using RealEstate.API.Middleware;
using RealEstate.Application;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Persistence.Seeders;
using RealEstate.Shared.Authorization;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

// ── Bootstrap logger — captures startup errors ────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting RealEstate API...");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ───────────────────────────────────────────
    builder.Host.UseSerilog((context, services, config) =>
        config
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId());

    // ── Controllers ───────────────────────────────────────
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter()));

    builder.Services.AddEndpointsApiExplorer();

    // ── Swagger ───────────────────────────────────────────
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "RealEstate SaaS API",
            Version = "v1"
        });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "أدخل الـ Token هنا"
        };

        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    },
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Name   = "Bearer",
                    In     = ParameterLocation.Header
                },
                Array.Empty<string>()
            }
        });
    });

    // ── Application + Infrastructure ──────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // ── JWT Authentication ────────────────────────────────
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };

        // ── Log auth failures ─────────────────────────────
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning(
                    "JWT authentication failed: {Error}",
                    context.Exception.Message);
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                Log.Warning(
                    "Forbidden access attempt to {Path}",
                    context.HttpContext.Request.Path);
                return Task.CompletedTask;
            }
        };
    });

    // ── Authorization ─────────────────────────────────────
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Policies.AdminAndUp, policy =>
            policy.RequireRole(Roles.Owner, Roles.Admin));

        options.AddPolicy(Policies.AgentAndUp, policy =>
            policy.RequireRole(Roles.Viewer, Roles.Admin, Roles.Agent, Roles.Owner));

        options.AddPolicy(Policies.SameCompany, policy =>
            policy.Requirements.Add(new SameCompanyRequirement()));
    });

    builder.Services.AddScoped<IAuthorizationHandler, SameCompanyHandler>();

    // ── CORS ──────────────────────────────────────────────
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:5174",
                    "http://localhost:5175")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
    });

    // ── wwwroot auto-create ───────────────────────────────
    var uploadsPath = Path.Combine(
        builder.Environment.WebRootPath ??
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
        "uploads");
    Directory.CreateDirectory(uploadsPath);

    var app = builder.Build();

    // ── Seed database (development only) ──────────────────
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<Program>>();
        await DatabaseSeeder.SeedAsync(context, logger);
    }

    // ── Middleware Pipeline ───────────────────────────────
    app.UseMiddleware<ExceptionMiddleware>();

    // ── Serilog request logging ───────────────────────────
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} → {StatusCode} ({Elapsed:0.0000}ms)";

        // Enrich with additional context
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent",
                httpContext.Request.Headers["User-Agent"].ToString());

            // Log CompanyId from JWT if available
            var companyId = httpContext.User
                .FindFirst("CompanyId")?.Value;
            if (!string.IsNullOrEmpty(companyId))
                diagnosticContext.Set("CompanyId", companyId);
        };

        // Skip health check and static file noise
        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            if (ex is not null)
                return Serilog.Events.LogEventLevel.Error;

            if (httpContext.Response.StatusCode >= 500)
                return Serilog.Events.LogEventLevel.Error;

            if (httpContext.Response.StatusCode >= 400)
                return Serilog.Events.LogEventLevel.Warning;

            if (httpContext.Request.Path.StartsWithSegments("/health"))
                return Serilog.Events.LogEventLevel.Debug;

            return Serilog.Events.LogEventLevel.Information;
        };
    });

    app.UseCors("AllowFrontend");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHangfireDashboard("/jobs");
    app.MapHangfireDashboard();

    // ── Schedule Hangfire Jobs ────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        var jobService = scope.ServiceProvider
            .GetRequiredService<INotificationJobService>();
        jobService.ScheduleJobs();
    }

    app.MapControllers();

    Log.Information("RealEstate API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "RealEstate API failed to start");
}
finally
{
    Log.CloseAndFlush();
}