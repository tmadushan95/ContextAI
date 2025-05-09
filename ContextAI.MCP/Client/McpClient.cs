using Anthropic.SDK;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

namespace ContextAI.MCP.Client
{

    public class McpClient(IConfiguration configuration)
    {
        /// <summary>
        /// The configuration object used to access the application settings.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Creates a Model Context Protocol (MCP) client using the StdioClientTransport.
        /// </summary>
        /// <returns></returns>
        public async Task CreateMcpClientAsync()
        {
            try
            {
                // Create a StdioClientTransport with the specified command and arguments.
                var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
                {
                    Name = "Everything",
                    Command = "npx",
                    Arguments = ["-y", "@modelcontextprotocol/server-everything"],
                });

                // Create a new MCP client using the StdioClientTransport.
                var client = await McpClientFactory.CreateAsync(clientTransport);

                // Get available functions.
                IList<McpClientTool> tools = await client.ListToolsAsync();

                // Print the list of tools available from the server.
                //foreach (var tool in tools)
                //{
                //    Console.WriteLine($"{tool.Name} ({tool.Description})");
                //}

                // Execute a tool (this would normally be driven by LLM tool invocations).
                var result = await client.CallToolAsync(
                    "echo",
                    new Dictionary<string, object?>() { ["message"] = "Hello MCP!" },
                    cancellationToken: CancellationToken.None);

                // echo always returns one and only one text content object
                Console.WriteLine(result.Content.First(c => c.Type == "text").Text);

                // Create a new AnthropicClient using the API key from the configuration.
                string key = _configuration.GetSection("McpClient").GetSection("AnthropicApiKey").Value ?? string.Empty;
                using var anthropicClient = new AnthropicClient(new APIAuthentication(key))
                                    .Messages
                                    .AsBuilder()
                                    .UseFunctionInvocation()
                                    .Build();

                // Create a new ChatOptions object with the specified parameters.
                var options = new ChatOptions
                {
                    MaxOutputTokens = 1000,
                    ModelId = "claude-3-5-sonnet-20241022",
                    Tools = [.. tools]
                };

                // Start the MCP client.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("MCP Client Started!");
                Console.ResetColor();

                // Get user input
                PromptForInput();
                while (Console.ReadLine() is string query && !"exit".Equals(query, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        PromptForInput();
                        continue;
                    }
                    
                    // Send the query to the MCP client and get the response.
                    await foreach (var message in anthropicClient.GetStreamingResponseAsync(query, options))
                    {
                        Console.Write(message);
                    }
                    Console.WriteLine();

                    PromptForInput();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating MCP client: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// <see langword="static"/> method to prompt for user input.
        /// </summary>
        static void PromptForInput()
        {
            Console.WriteLine("Enter a command (or 'exit' to quit):");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            Console.ResetColor();
        }
    }
}
