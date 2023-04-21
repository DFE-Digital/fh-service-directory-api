using Serilog;

namespace FamilyHubs.ServiceDirectory.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Starting up");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureHost();

            builder.Services.RegisterApplicationComponents(builder.Configuration);

            builder.Services.ConfigureServices(builder.Configuration, builder.Environment.IsProduction());
            
            var webApplication = builder.Build();

            await webApplication.ConfigureWebApplication();

            await webApplication.RunAsync();
        }
        catch (Exception e)
        {
            if (e.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
            {
                //this error only occurs when DB migration is running on its own
                throw;
            }
            
            Log.Fatal(e, "An unhandled exception occurred during bootstrapping");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}