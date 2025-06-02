using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContextAI.MCP.Server.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddMCPServices(); // Properly defined below
                })
                .UseConsoleLifetime(); // Ensures graceful shutdown on Ctrl+C or SIGTERM
    }
}
