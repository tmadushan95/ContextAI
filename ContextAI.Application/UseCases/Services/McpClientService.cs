using ContextAI.Application.Interfaces.IServices;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using System.Diagnostics;
using System.Text.Json;
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
                    var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Tool);

                    if (assistantMessage is not null)
                    {
                        var textOutput = ExtractedResponseText(assistantMessage);
                        //var textOutput = string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()));
                        responseMessages.Add(textOutput);
                    }
                    else
                    {
                        responseMessages.Add("No message received.");
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


        /// <summary>
        /// Extracts the text content from the result of a <see cref="ChatMessage"/> 
        /// assuming the result is a JSON object with a "content" array containing 
        /// objects with a "text" property.
        /// </summary>
        /// <param name="chatMessage">The <see cref="ChatMessage"/> containing AI response content.</param>
        /// <returns>
        /// A string containing the extracted text from the response, or a fallback message
        /// if the expected structure is not present or no content is found.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws any exception encountered during JSON processing to preserve stack trace 
        /// and allow higher-level handling.
        /// </exception>
        public static string ExtractedResponseText(ChatMessage chatMessage)
        {
            try
            {
                // Ensure we have a valid list of contents
                IList<AIContent> contents = chatMessage.Contents;
                if (contents == null || contents.Count == 0)
                {
                    return "No message received."; // No content to process
                }

                // Safely cast the first content item to FunctionResultContent
                var resultContent = contents[0] as FunctionResultContent;
                if (resultContent?.Result is not JsonElement jsonElement)
                {
                    return "No message received."; // Content was not in expected format
                }

                // Attempt to extract the "content" array from the JSON result
                if (jsonElement.TryGetProperty("content", out JsonElement contentElement) &&
                    contentElement.ValueKind == JsonValueKind.Array &&
                    contentElement.GetArrayLength() > 0)
                {
                    JsonElement firstItem = contentElement[0];

                    // Attempt to extract the "text" field from the first item in the array
                    if (firstItem.TryGetProperty("text", out JsonElement textElement) &&
                        textElement.ValueKind == JsonValueKind.String)
                    {
                        // Return the extracted text
                        return textElement.GetString() ?? string.Empty;
                    }
                }

                // If structure is present but doesn't contain expected fields
                return "No valid text found in response.";
            }
            catch (Exception ex)
            {
                // Log detailed exception to debug output (or real logging system in production)
                Debug.WriteLine($"Error extracting response text: {ex}");
                throw; // Re-throw to preserve stack trace and signal failure upstream
            }
        }

    }
}
