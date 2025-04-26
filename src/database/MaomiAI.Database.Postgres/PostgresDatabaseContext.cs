using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database.Postgres;

public class PostgresDatabaseContext : DatabaseContext
{
    public PostgresDatabaseContext(DbContextOptions options, IServiceProvider serviceProvider, DatabaseOptions contextOptions) : base(options, serviceProvider, contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // postgres 需要开启此扩展，以便支持 uuid_generate_v4()
        modelBuilder.HasPostgresExtension("uuid-ossp");

        base.OnModelCreating(modelBuilder);
    }
}
