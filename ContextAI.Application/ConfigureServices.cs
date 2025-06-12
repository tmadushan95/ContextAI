using ContextAI.Application.Interfaces.IServices;
using ContextAI.Application.UseCases.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ContextAI.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddContextAIApplicationServices(this IServiceCollection services)
        {
            try
            {
                #region Service Configuration
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
