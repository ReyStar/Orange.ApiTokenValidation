using System;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Orange.ApiTokenValidation.DataBaseProducer
{
    //TODO think things
    public class Program
    {
        private const string AppSettingPrefix = "appsettings";

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var serviceProvider = CreateServices(args);

                // Put the database update into a scope to ensure
                // that all resources will be disposed.
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbCreator = scope.ServiceProvider.GetService<DataBaseCreator>();
                    await dbCreator.RunAsync();

                    // Instantiate the runner
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                    // Execute the migrations
                    runner.MigrateUp();
                }

                Console.WriteLine("Migration completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("migration completed with errors");
                Console.WriteLine(ex);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices(string[] args)
        {
            var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(applicationPath)
                            .AddJsonFile($"{AppSettingPrefix}.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args);
            var configurationRoot = builder.Build();

            var connectionString = configurationRoot.GetConnectionString("DefaultConnection");

            // Add common FluentMigrator services
            return new ServiceCollection().AddSingleton(configurationRoot)
                                          .AddSingleton<DataBaseCreator>()
                                          .AddFluentMigratorCore()
                                          .ConfigureRunner(rb =>
                                          {
                                              // Add Postgres support to FluentMigrators
                                              rb.AddPostgres();
                                              // Set the connection string
                                              rb.WithGlobalConnectionString(connectionString);
                                              // Define the assembly containing the migrations
                                              rb.ScanIn(typeof(Program).Assembly).For.Migrations();
                                          })
                                          // Enable logging to console in the FluentMigrator way
                                          .AddLogging(lb => lb.AddFluentMigratorConsole())
                                          // Build the service provider
                                          .BuildServiceProvider(false);
        }
    }
}
