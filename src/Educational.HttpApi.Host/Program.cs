using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Educational;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
           //Services.AddMvc().AddJsonOptions(options =>
           // {
           //     options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // 保留原名[6](@ref)
           // });

            Log.Information("Starting Educational.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<EducationalHttpApiHostModule>();

           
            var app = builder.Build();

            app.UseCors(a=>a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            await app.InitializeApplicationAsync();
            await app.RunAsync();



            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
