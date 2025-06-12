using ContextAI.Application.Interfaces.IServices;
using ContextAI.Application.UseCases.Services;
using ContextAI.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ContextAI.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddContextAIApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                #region Appsetting Configuration
                // Binds the "McpServerConfig" section from the "McpConfiguration" section 
                // in appsettings.json to the McpServerConfiguration class.
                services.Configure<McpServerConfiguration>(configuration.GetSection("McpConfiguration").GetSection("McpServerConfig"));
                #endregion

                #region Service Configuration
                // Registers the IMcpClientService interface and its implementation McpClientService
                // with scoped lifetime for dependency injection.
                services.AddScoped<IMcpClientService, McpClientService>();
                #endregion

                return services;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
