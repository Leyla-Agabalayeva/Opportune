using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Opportune.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

        services.AddDbContext<AppDbContext>(options => options.UseOpportunePostgres(connectionString));
        return services;
    }

    /// <summary>The one place that decides how the database is reached; tests reuse it.</summary>
    public static DbContextOptionsBuilder UseOpportunePostgres(this DbContextOptionsBuilder options, string connectionString) =>
        options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

    public static DbContextOptionsBuilder<TContext> UseOpportunePostgres<TContext>(
        this DbContextOptionsBuilder<TContext> options, string connectionString)
        where TContext : DbContext
    {
        ((DbContextOptionsBuilder)options).UseOpportunePostgres(connectionString);
        return options;
    }
}
