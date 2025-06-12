using ContextAI.Application.Interfaces.IServices;
using ContextAI.Domain.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using System.Diagnostics;
namespace ContextAI.Application.UseCases.Services
{
    public class McpClientService(IOptions<McpServerConfiguration> _mcpServerConfiguration) : IMcpClientService
    {
        /// <summary>
        /// The base path of the MCP server, obtained from configuration.
        /// </summary>
        private readonly string _mcpServerPath = _mcpServerConfiguration.Value.McpServerPath;

        /// <summary>
        /// Starts the MCP client, retrieves available tools from the MCP server,
        /// invokes the Ollama LLM using user input, and returns the assistant's response.
        /// </summary>
        /// <param name="userInput">The prompt to send to the language model.</param>
        /// <returns>A list of response strings from the assistant.</returns>
        public async Task<List<string>> StartMcpClientAsync(string userInput)
        {
            try
            {
                // Initialize a transport client to communicate with the MCP server over stdio.
                var clientTransport = new StdioClientTransport(new()
                {
                    Name = "Demo Server",            // Optional descriptive name for the transport.
                    Command = _mcpServerPath         // Path to the MCP server executable.
                });

                // Create and initialize the MCP client using the transport.
                await using var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

                // Set up the Ollama chat client (LLM) with the specified model and endpoint.
                var ollamaChatClient = new OllamaChatClient(
                    new Uri("http://localhost:11434/"),     // Ollama local server endpoint.
                    "llama3.2:3b"                           // Model name to use.
                );

                // Build a ChatClient with function invocation support, using the Ollama chat client.
                var chatClient = new ChatClientBuilder(ollamaChatClient)
                        .UseFunctionInvocation()                // Enable tools/functions support in the chat.
                        .Build();

                // Fetch the list of available tools exposed by the MCP server.
                var mcpTools = await mcpClient.ListToolsAsync();

                // This list will collect the assistant's responses to return at the end.
                List<string> responseMessages = [];

                // Prepare the initial message list: a system prompt and the user's input.
                var messages = new List<ChatMessage>
                {
                    new(ChatRole.System, "You are a helpful assistant."), // Set assistant behavior.
                    new(ChatRole.User, userInput)                         // User's question/input.
                };


                try
                {
                    // Send the message list to the chat client, passing in the MCP tools for use.
                    var response = await chatClient.GetResponseAsync(
                        messages,
                        new ChatOptions { Tools = [.. mcpTools] }); // Spread MCP tools into options.

                    // Extract the assistant's reply (if any) from the response messages.
                    var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Assistant);

                    if (assistantMessage != null)
                    {
                        // Convert the assistant's message content to a single string and add to results.
                        var textOutput = string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()));
                        responseMessages.Add(textOutput);
                    }
                    else
                    {
                        // Handle case where no assistant message was received.
                        responseMessages.Add("AI: (no assistant message received)");
                    }
                }
                catch (Exception ex)
                {
                    // If there's an error during interaction with the chat client, record the error.
                    responseMessages.Add($"Error: {ex.Message}");
                }

                return responseMessages;
            }
            catch (Exception ex)
            {
                // Log unexpected errors to debug output, then rethrow to propagate the exception.
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
