namespace PicPay.Api.Database;

public static class DbContextExtensions
{
    public static async Task SaveTasksAsync(this PicPayDbContext ctx, Guid eventId, params IPicPayTask[] tasks)
    {
        foreach (var task in tasks)
        {
            ctx.Tasks.Add(new PicPayTask(eventId, task));
        }

        await ctx.SaveChangesAsync();
    }

    public static async Task<string?> GetWalletOwnerName(this PicPayDbContext ctx, Guid walletId)
    {
        FormattableString sql = @$"
            SELECT name
            FROM picpay.users u
            INNER JOIN picpay.wallets w ON w.user_id = u.id
            WHERE w.id = {walletId}
        ";

        return await ctx.Users.FromSql(sql).Select(x => x.Name).FirstOrDefaultAsync();
    }

    public static void ResetDb(this PicPayDbContext ctx)
    {
        if (!Env.IsTesting())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }

    public static async Task ResetDbAsync(this PicPayDbContext ctx)
    {
        if (Env.IsTesting())
        {
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
        }
    }
}
