using Dapper;
using Npgsql;
using Hangfire;
using PicPay.Worker.Extensions;

namespace PicPay.Worker.Tasks;

public class PicPayTasksProcessorDbListener(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dataSource = NpgsqlDataSource.Create(configuration.Database().ConnectionString);
        var connection = await dataSource.OpenConnectionAsync(stoppingToken);

        await CreateTrigger(connection);

        connection.Notification += (o, e) =>
        {
            var processingJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(PicPayTasksProcessor));
            var enqueuedJobs = JobStorage.Current.GetMonitoringApi().EnqueuedJobs("default", 0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(PicPayTasksProcessor));
            if (processingJobs < 15 && enqueuedJobs < 5)
            {
                BackgroundJob.Enqueue<PicPayTasksProcessor>(x => x.Run());
            }
        };

        await using (var cmd = new NpgsqlCommand("LISTEN new_task;", connection))
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
            CREATE OR REPLACE FUNCTION notify_new_task_trigger()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$ 
            BEGIN
                PERFORM pg_notify('new_task', '');
                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER notify_new_task_trigger
            AFTER INSERT ON picpay.tasks
            EXECUTE PROCEDURE notify_new_task_trigger();
        ";

        await connection.ExecuteAsync(sql);
    }
}
