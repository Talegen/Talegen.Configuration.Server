/*
 * Talegen Configuration Server
 * (c) Copyright Talegen, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

namespace Talegen.Configuration.Server
{
    using Models.Configuration;
    using Talegen.Configuration.Server.Hubs;
    using Serilog;

    public class Program
    {
        /// <summary>
        /// Contains the value for max number of files to import. The default value is 1024.
        /// </summary>
        public const int MaxNumberOfFilesToImport = 8192;

        /// <summary>
        /// Defines the default cors policy name.
        /// </summary>
        private const string DefaultCorsPolicy = "default";

        /// <summary>
        /// Defines the notifications hub route.
        /// </summary>
        private const string NotificationHubRoute = "/hubs/notificationHub";

        /// <summary>
        /// Contains the configuration root loader.
        /// </summary>
        private static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the application settings for the program.
        /// </summary>
        public static ServerSettings Settings { get; set; }

        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">Contains an array of command line arguments.</param>
        public static void Main(string[] args)
        {
            Console.Title = Properties.Resources.ApplicationName;

            // get the connection string information early
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

            // set the configuration instance.
            Configuration = configurationBuilder.Build();

            // setup logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            // get application settings
            var settingsSection = Configuration.GetSection("ApplicationSettings");
            Settings = settingsSection.Get<ServerSettings>();

            // setup web application builder
            var builder = WebApplication.CreateBuilder(args);

            ConfigureService(builder, builder.Environment.IsDevelopment());

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR(config =>
            {
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapDefaultControllerRoute();

            // map configuration hub to endpoint.
            app.MapHub<ConfigurationSyncHub>("/channel/configuration", options =>
            {
            }).RequireAuthorization();

            app.Run();
        }

        /// <summary>
        /// This method is used to orchestrate service configuration.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="isDevelopment"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void ConfigureService(WebApplicationBuilder builder, bool isDevelopment)
        {
            throw new NotImplementedException();
        }
    }
}