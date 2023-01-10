using Discord;
using Discord.Commands;
using Discord.WebSocket;
using dpbc.app.Services;
using dpbc.repository.Repository;
using dpbc.service.Service;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace dpbc.app
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                using (var scope = services.CreateScope())
                {
                    UpdateDatabase(scope.ServiceProvider);
                }

                var client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;
                await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN_DISCORD"));
                await client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            var conn = Environment.GetEnvironmentVariable("conn");

            return new ServiceCollection()
                .AddSingleton(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<IDBContext>(s => new DBContext(conn!))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IPointService, PointService>()
                .AddScoped<IUserService, UserService>()
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c
                    .AddSqlServer()
                    .WithGlobalConnectionString(conn)
                    .ScanIn(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("dpbc.repository")).ToArray()).For.Migrations())
                .AddLogging(c => c.AddFluentMigratorConsole())
                .BuildServiceProvider();
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
