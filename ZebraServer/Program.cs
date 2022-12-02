using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Zebra.Library;
using Microsoft.EntityFrameworkCore;
using Zebra.Library.PdfHandling;
using Zebra.Library.Services;
using Microsoft.OpenApi.Models;
using System.Threading;
using System.IO;

namespace ZebraServer
{
    public class ZebraServer
    {

        private WebApplication _webApplication;

        /// <summary>
        /// Command Line Arguments
        /// Can only be set when run from Command Line
        /// </summary>
        public string[] Args { get; private set; }

        /// <summary>
        /// Path to ZebraConfig Configuration File for Server
        /// If not set, the path defaults to ./zebraconfig.json
        /// </summary>
        public FileInfo ConfigurationFilePath { get; set; }

        public static async Task Main(string[] args)
        {
            var server = new ZebraServer();
            server.Args = args;

            server.ConfigureInstance();

            await server.RunAsync();
       
        }

        /// <summary>
        /// Creates new Instance of Zebra Server
        /// Define Configuration File path by setting Property ConfigurationFilePath
        /// </summary>
        public ZebraServer()
        {
            ConfigurationFilePath = new FileInfo(Path.Combine(AppContext.BaseDirectory, "zebraconfig.json"));
        }

        /// <summary>
        /// Wrapper around the WebApplication.StartAsync() call.
        /// This method is used to start the Webserver from another application, e.g. ZebraDesktop
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken token = default)
        {
            // The instance has to be configured every time before it is stared
            // Otherwise, the instance throws an Operation Cancelled Exception
            ConfigureInstance();
            await _webApplication.StartAsync(token);
        }

        /// <summary>
        /// Wrapper around the WebApplication.StopAsync() call.
        /// This method is used to stop the Webserver from another application, e.g. ZebraDesktop
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken token = default)
        {
            await _webApplication.StopAsync(token);
        }

        /// <summary>
        /// Internal wrapper around WebApplication.RunAsync()
        /// To be used by standalone Zebra Server
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task RunAsync(CancellationToken token = default)
        {
            ConfigureInstance();
            await _webApplication.RunAsync(token);
        }

        /// <summary>
        /// Sets up the internal instance of WebApplication
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ConfigureInstance()
        {
            var builder = WebApplication.CreateBuilder(this.Args);

            // Configure Configuration File

            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Sources.Clear();

                var env = hostingContext.HostingEnvironment;


                // Support for both, XML and JSON Files

                switch (ConfigurationFilePath.Extension)
                {
                    case ".json":
                       config.AddJsonFile(ConfigurationFilePath.FullName,
                                    optional: false,
                                        reloadOnChange: true);
                        break;
                    case ".xml":
                        config.AddXmlFile(ConfigurationFilePath.FullName,
                                    optional: false,
                                        reloadOnChange: true);
                        break;
                    default:
                        throw new Exception("Configuration File format was neither JSON nor XML");
                }
                
                //config.AddEnvironmentVariables();

                if (this.Args != null)
                {
                    config.AddCommandLine(this.Args);
                }
            });

            // Configure Services

            builder.Services.AddDbContext<ZebraContext, SQLiteZebraContext>();


            builder.Services.AddScoped<IImportCandidateImporter, ImportCandidateImporter>();
            builder.Services.AddTransient<FileNameService>();
            builder.Services.AddTransient<ArchiveService>();
            builder.Services.AddSingleton<ZebraConfigurationService>();

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZebraServer", Version = "v1" });
            });



            var app = builder.Build();



            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZebraServer v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Setup ZebraConfig Configuration Service
            // This Service loads the provided Configuration File and exposes the paths

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {

                    var section = builder.Configuration.GetSection(nameof(ZebraConfig));

                    var config = section.Get<ZebraConfig>();
                    if (config is null) throw new Exception("Configuration file could not be read.");

                    app.Services.GetRequiredService<ZebraConfigurationService>().LoadConfiguration(config);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<ZebraServer>>();
                    logger.LogError(ex, "An error occurred while setting up the configuration service.");
                    logger.LogError("Quitting Application");
                    return;

                }
            }

            // Create and populate DB if it doesn't exist

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ZebraContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<ZebraServer>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    logger.LogError("Quitting Application");
                    return;

                }

            }


            // Set internal property to newly created App

            _webApplication = app;
        }
    }
}
