using ContextAI.Application.Interfaces.IServices;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using System.Diagnostics;
namespace ContextAI.Application.UseCases.Services
{
    public class McpClientService(IChatClient chatClient, IMcpClient mcpClient) : IMcpClientService
    {
        private readonly IChatClient _chatClient = chatClient;
        private readonly IMcpClient _mcpClient = mcpClient;

        /// <summary>
        /// Executes a full interaction cycle with the MCP server and the LLM:
        /// - Retrieves tools from the MCP server
        /// - Sends user input and system context to the chat client
        /// - Returns the assistant’s response (if available)
        /// </summary>
        /// <param name="userInput">User's prompt or message</param>
        /// <returns>List of assistant response strings</returns>
        public async Task<List<string>> GenerateAssistantResponseAsync(string userInput)
        {
            var responseMessages = new List<string>();

            try
            {
                // Step 1: Retrieve available tools from the MCP server
                var mcpTools = await _mcpClient.ListToolsAsync();

                // Step 2: Construct the initial conversation context
                var messages = new List<ChatMessage>
                {
                    new(ChatRole.System, "You are a helpful assistant."),
                    new(ChatRole.User, userInput)
                };

                try
                {
                    // Step 3: Send input and tools to the chat client and await the response
                    var response = await _chatClient.GetResponseAsync(
                        messages,
                        new ChatOptions { Tools = [.. mcpTools] }
                    );

                    // Step 4: Extract the assistant's message, if any
                    var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Assistant);

                    if (assistantMessage is not null)
                    {
                        var textOutput = string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()));
                        responseMessages.Add(textOutput);
                    }
                    else
                    {
                        responseMessages.Add("AI: No assistant message received.");
                    }
                }
                catch (Exception ex)
                {
                    // Capture errors specific to chat interaction
                    responseMessages.Add($"Error during chat interaction: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Log and rethrow for upstream visibility (monitoring/logging can be added here)
                Debug.WriteLine($"[McpClientService] Unexpected error: {ex}");
                throw;
            }

            return responseMessages;
        }
    }
}
