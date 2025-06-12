using ContextAI.Application.Interfaces.IServices;
using ContextAI.Application.UseCases.Services;
using ContextAI.Domain.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Client;
using System.Diagnostics;

namespace ContextAI.Application
{
    public static class ConfigureServices
    {
        /// <summary>
        /// Application Services Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddContextAIApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                #region Service Configuration
                // Retrieve configuration settings for mcp server
                var _mcpServerConfig = configuration.GetSection("McpConfiguration")
                                            .GetSection("McpServerConfig")
                                            .Get<McpServerConfiguration>()!; ;

                // Register IMcpClient as a scoped service in the dependency injection container.
                // The client communicates with the MCP server over standard input/output (Stdio).
                services.AddScoped<IMcpClient>(provider =>
                {
                    // Initialize a transport layer to launch and communicate with the MCP server via a shell command.
                    var clientTransport = new StdioClientTransport(new()
                    {
                        Name = "ContextAI Server",
                        Command = _mcpServerConfig.McpServerPath,
                    });

                    // Create and return the MCP client instance using the configured transport.
                    return McpClientFactory.CreateAsync(clientTransport).GetAwaiter().GetResult();
                });

                // Retrieve configuration settings for mcp client
                var _mcplientConfig = configuration.GetSection("McpConfiguration")
                                             .GetSection("McpClientConfig")
                                             .Get<McpClientConfiguration>()!;

                // Register IChatClient as a scoped service, configured to use Ollama and enable function calling.
                services.AddScoped<IChatClient>(provider =>
                {
                    // Instantiate a chat client using Ollama's endpoint and model.
                    var ollamaChatClient = new OllamaChatClient(
                           new Uri(_mcplientConfig.OllamaEndpoint),     // Local or remote Ollama server URL.
                           _mcplientConfig.OllamaModelName              // Name of the model to be used (e.g., llama3, codellama).
                    );

                    // Use a builder pattern to enable function invocation and return the constructed chat client.
                    return new ChatClientBuilder(ollamaChatClient)
                                    .UseFunctionInvocation()
                                    .Build();
                });

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
