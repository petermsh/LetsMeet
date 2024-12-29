using System.Text;
using System.Text.Json.Serialization;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Application.Hubs;
using LetsMeet.Domain.Entities;
using LetsMeet.Infrastructure.Data;
using LetsMeet.Infrastructure.Data.Tools;
using LetsMeet.Infrastructure.Options;
using LetsMeet.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LetsMeet.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<AppExceptionHandler>();

        services.AddCors();
        
        services.AddSingleton(TimeProvider.System);
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        services
            .SetUpDatabase(configuration)
            .SetUpUserAndAuth(configuration)
            .SetUpSwagger();

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseExceptionHandler(opt => {});

        app.UseCors(cp =>
        {
            cp.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
        
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();

        app.MapHub<ChatHub>("/chat");
        
        app
            .UseAuthentication()
            .UseAuthorization();

        return app;
    }

    private static IServiceCollection SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<DbMigrator>()
            .AddTransient<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        var connectionString = configuration.GetConnectionString("LetsMeetDb");
        services.AddDbContext<DataContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());

        return services;
    }

    private static IServiceCollection SetUpUserAndAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        var jwtOption = configuration.GetSection("JWT").Get<JwtSettings>();
        services.AddSingleton(jwtOption);

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOption.SecretKey)),
                    ValidIssuer = jwtOption.Issuer,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection SetUpSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        //services.AddFluentValidationRulesToSwagger();
        services.AddEndpointsApiExplorer();

        services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(opt =>
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}