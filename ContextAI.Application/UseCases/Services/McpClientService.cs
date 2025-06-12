using ContextAI.Application.Interfaces.IServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using System.Diagnostics;
using System.Text.Json;
namespace ContextAI.Application.UseCases.Services
{
    public class McpClientService : IMcpClientService
    {
        public async Task<List<string>> StartMcpClientAsync(string userInput)
        {
            try
            {
                var clientTransport = new StdioClientTransport(new()
                {
                    Name = "Demo Server",
                    Command = "D:\\Project\\ContextAI\\ContextAI\\ContextAI\\ContextAI.MCP.Server\\bin\\Debug\\net8.0\\ContextAI.MCP.Server.exe"
                });

                // Logger
                using var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddConsole().SetMinimumLevel(LogLevel.Information));

                await using var mcpClient = await McpClientFactory.CreateAsync(clientTransport, loggerFactory: loggerFactory);

                // Configure Ollama LLM Client
                var ollamaChatClient = new OllamaChatClient(
                    new Uri("http://localhost:11434/"),
                    "llama3.2:3b"
                );

                var chatClient = new ChatClientBuilder(ollamaChatClient)
                        .UseLogging(loggerFactory)
                        .UseFunctionInvocation()
                        .Build();

                // Get available tools from MCP Server
                var mcpTools = await mcpClient.ListToolsAsync();
                foreach (var tool in mcpTools)
                {
                    Console.WriteLine($"Connected to server with tools: {tool.Name}");
                }

                var toolsJson = JsonSerializer.Serialize(mcpTools, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("\nAvailable Tools:\n" + toolsJson);

                await Task.Delay(100);


                List<string> message = [];

                var messages = new List<ChatMessage>
                    {
                        new(ChatRole.System, "You are a helpful assistant."),
                        new(ChatRole.User, userInput)
                    };


                try
                {
                    var response = await chatClient.GetResponseAsync(
                        messages,
                        new ChatOptions { Tools = [.. mcpTools] });

                    var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Assistant);

                    if (assistantMessage != null)
                    {
                        var textOutput = string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()));
                        message.Add(textOutput);
                        Console.WriteLine("\n AI: " + textOutput);
                    }
                    else
                    {
                        message.Add("\n AI: (no assistant message received)");
                    }
                }
                catch (Exception ex)
                {
                    message.Add($"\n Error: {ex.Message}");
                }


                return message;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
