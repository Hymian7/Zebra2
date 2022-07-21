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

namespace ZebraServer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Configure Configuration File

            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Sources.Clear();

                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("zebraconfig.json",
                       optional: false,
                       reloadOnChange: true);

                //config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Setup Configuration Service

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
                    var logger = services.GetRequiredService<ILogger<Program>>();
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
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    logger.LogError("Quitting Application");
                    return;

                }


                // Run the app

                app.Run();

                

                //ZebraContext dbcontext = app.Services.GetRequiredService<ZebraContext>();
                //dbcontext.Database.EnsureCreated();

            }

            //public static IHostBuilder CreateHostBuilder(string[] args) =>
            //    Host.CreateDefaultBuilder(args)
            //        .ConfigureWebHostDefaults(webBuilder =>
            //        {
            //            webBuilder.UseStartup<Startup>();
            //        });
        }
    }
}
