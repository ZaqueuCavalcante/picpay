using PicPay.Api.Features.Cross.CreateUser;
using PicPay.Api.Features.Cross.CreateTransaction;
using PicPay.Api.Features.Cross.CreateNotification;

namespace PicPay.Api.Database;

public class PicPayDbContext(DbContextOptions<PicPayDbContext> options, DatabaseSettings settings) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<PicPayUser> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(settings.ConnectionString);
        optionsBuilder.AddInterceptors(new SaveDomainEventsInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("picpay");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    public async Task SaveTasksAsync(Guid eventId, params IPicPayTask[] tasks)
    {
        foreach (var task in tasks)
        {
            Add(new PicPayTask(eventId, task));
        }

        await SaveChangesAsync();
    }

    public void ResetDb()
    {
        if (!Env.IsTesting())
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }

    public async Task ResetDbAsync()
    {
        if (Env.IsTesting())
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
        }
    }
}
