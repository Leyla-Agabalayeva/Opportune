using Microsoft.EntityFrameworkCore;
using Opportune.Persistence;
using Testcontainers.PostgreSql;

namespace Opportune.Tests.Persistence;

/// <summary>
/// Starts one real PostgreSQL in Docker for all tests of the collection and applies
/// the migrations to it, so the tests exercise the actual schema and constraints.
/// </summary>
public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17").Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var db = NewContext();
        await db.Database.MigrateAsync();
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    public AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseOpportunePostgres(_container.GetConnectionString())
            .Options;

        return new AppDbContext(options);
    }
}

[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "postgres";
}
