using LetsMeet.Infrastructure.Data;
using LetsMeet.Infrastructure.Data.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsMeet.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();
        services.SetUpDatabase(configuration);

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        return app;
    }

    private static IServiceCollection SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<DbMigrator>()
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        var connectionString = configuration.GetConnectionString("LetsMeetDb");
        services.AddDbContext<DataContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
}