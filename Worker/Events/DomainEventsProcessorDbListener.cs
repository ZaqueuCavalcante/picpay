using Dapper;
using Npgsql;
using Hangfire;
using PicPay.Worker.Extensions;

namespace PicPay.Worker.Events;

public class DomainEventsProcessorDbListener(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dataSource = NpgsqlDataSource.Create(configuration.Database().ConnectionString);
        var connection = await dataSource.OpenConnectionAsync(stoppingToken);

        await CreateTrigger(connection);

        connection.Notification += (o, e) =>
        {
            BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());
        };

        await using (var cmd = new NpgsqlCommand("LISTEN new_domain_event;", connection))
        {
            await cmd.ExecuteNonQueryAsync(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await connection.WaitAsync(stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    private static async Task CreateTrigger(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE OR REPLACE FUNCTION notify_new_domain_event_trigger()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$ 
            BEGIN
                PERFORM pg_notify('new_domain_event', '');
                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER notify_new_domain_event_trigger
            AFTER INSERT ON picpay.domain_events
            FOR EACH ROW EXECUTE PROCEDURE notify_new_domain_event_trigger();
        ";

        await connection.ExecuteAsync(sql);
    }
}
