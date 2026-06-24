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
using RealEstate.Shared.Authorization;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Define the Security Scheme
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme, // "Bearer" using the formal constant
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "أدخل الـ Token هنا"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    // 2. Updated .NET 10 Swashbuckle v10 syntax for security requirements
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                // Crucial fix for .NET 10 / OpenApi v2.x schema alignment
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication
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
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.OwnerOnly, policy =>
        policy.RequireRole(Roles.Owner));

    options.AddPolicy(Policies.AdminAndUp, policy =>
        policy.RequireRole(Roles.Owner, Roles.Admin));

    options.AddPolicy(Policies.AgentAndUp, policy =>
        policy.RequireRole(Roles.Owner, Roles.Admin, Roles.Agent));

    options.AddPolicy(Policies.SameCompany, policy =>
        policy.Requirements.Add(new SameCompanyRequirement()));
});

builder.Services.AddScoped<IAuthorizationHandler, SameCompanyHandler>();

// أضف قبل builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:5175"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware Pipeline — الترتيب مهم جداً
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();  // ← أولاً
app.UseAuthorization();   // ← ثانياً
app.UseHangfireDashboard("/jobs");
app.MapHangfireDashboard();

// Hangfire Jobs
using (var scope = app.Services.CreateScope())
{
    var jobService = scope.ServiceProvider
        .GetRequiredService<INotificationJobService>();
    jobService.ScheduleJobs();
}

app.MapControllers();
app.Run();