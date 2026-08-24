using OnlineStore.Infrastructure.Persistence;
using OnlineStore.Infrastructure.Persistence.Seed;
using OnlineStore.Infrastructure.Persistence.Seed.SeedData;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly OnlineStoreDbContext _context;
    private readonly IdentitySeeder _identitySeeder;
    private readonly CatalogSeed _catalogSeed;
    public DatabaseSeeder(
        OnlineStoreDbContext context,
        IdentitySeeder identitySeeder,
        CatalogSeed catalogSeed)
    {
        _context = context;
        _identitySeeder = identitySeeder;
        _catalogSeed = catalogSeed;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Database.CanConnectAsync())
            return;

        await _identitySeeder.SeedAsync();
        await _catalogSeed.SeedAsync();
    }
}