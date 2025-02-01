using Dapper;
using Npgsql;

namespace PicPay.Api.Features.Cross.GetExtract;

public class GetExtractService(DatabaseSettings settings) : IPicPayService
{
    public async Task<List<GetExtractOut>> Get(Guid walletId)
    {
        var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                t.id,
                t.amount * -1 AS amount,
                t.type,
                u.name AS other,
                t.created_at
            FROM
                picpay.transactions t
            INNER JOIN
                picpay.wallets w ON w.id = t.target_wallet_id
            INNER JOIN
                picpay.users u ON u.id = w.user_id
            WHERE
                t.source_wallet_id = @WalletId

            UNION

            SELECT
                t.id,
                t.amount AS amount,
                t.type,
                u.name AS other,
                t.created_at
            FROM
                picpay.transactions t
            INNER JOIN
                picpay.wallets w ON w.id = t.source_wallet_id
            INNER JOIN
                picpay.users u ON u.id = w.user_id
            WHERE
                t.target_wallet_id = @WalletId
        ";

        var transactions = await connection.QueryAsync<GetExtractOut>(sql, new { walletId });

        return transactions.OrderByDescending(x => x.CreatedAt).ToList();
    }
}
