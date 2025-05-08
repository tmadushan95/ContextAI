using McpDotNet;
using Microsoft.Extensions.DependencyInjection;

namespace ContextAI.MCPServer
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
