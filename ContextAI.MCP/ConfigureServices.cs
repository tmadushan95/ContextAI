using ContextAI.MCP.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ContextAI.MCP
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddMCPServices(this IServiceCollection services)
        {
            try
            {
                #region MCP server Configuration
                services.AddMcpServer()
                    .WithStdioServerTransport()
                    .WithToolsFromAssembly();
                #endregion

                #region MCP client Configuration
                services.AddSingleton<McpClient>();
                #endregion

                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
