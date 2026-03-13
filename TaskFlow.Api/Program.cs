using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using TaskFlow.Api.Mapping;
using TaskFlow.Api.Middleware;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Repository;
using TaskFlow.Infrastructure.Security;

#region Bootstrap Logger

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

#endregion

try
{
    Log.Information("Starting TaskFlow API");

    var builder = WebApplication.CreateBuilder(args);

    #region Serilog Configuration

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

    #endregion

    #region Controllers

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    builder.Services.AddOpenApi();

    #endregion

    #region Database

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("Database connection string not configured");
    }

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);

        if (builder.Environment.IsDevelopment())
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        }
    });

    #endregion

    #region Identity

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    #endregion

    #region Dependency Injection

    builder.Services.AddScoped<ITaskRepository, TaskRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
    builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
    builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IOrganizationService, OrganizationService>();
    builder.Services.AddScoped<IMembershipService, MembershipService>();

    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    #endregion

    #region JWT Authentication

    builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetSection("JwtSettings"));

    builder.Services.AddSingleton<IJwtOptions>(sp =>
        sp.GetRequiredService<IOptions<JwtOptions>>().Value);

    var jwtOptions = builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtOptions>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning(context.Exception,
                    "JWT authentication failed");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Log.Warning("Unauthorized request to {Path}",
                    context.Request.Path);
                return Task.CompletedTask;
            }
        };
    });

    #endregion

    #region AutoMapper

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MappingProfile>();
    });

    #endregion

    #region Cache

    builder.Services.AddMemoryCache();

    #endregion

    #region CORS

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    #endregion

    var app = builder.Build();

    #region Database Initialization

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            await DbInitializer.InitializeAsync(services);
            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Database initialization failed");
        }
    }

    #endregion

    #region Middleware

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSerilogRequestLogging();

    #endregion

    #region Development Tools

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    #endregion

    app.UseHttpsRedirection();

    app.UseCors("AllowAngular");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("TaskFlow API started");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}