using Microsoft.Extensions.DependencyInjection;

namespace ContextAI.MCP.Server
{
    /// <summary>
    /// Provides extension methods to configure services for the Model Context Protocol (MCP) server.
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Registers the necessary services for the MCP server in the provided service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
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
