namespace ContextAI.Domain.Helpers
{
    /// <summary>
    /// Configuration settings for the MCP server.
    /// </summary>
    public class McpServerConfiguration
    {
        /// <summary>
        /// Gets or sets the file system path to the MCP server executable or script.
        /// </summary>
        public string McpServerPath { get; set; } = null!;
    }

    /// <summary>
    /// Configuration settings for the MCP client, specifically for interacting with the Ollama service.
    /// </summary>
    public class McpClientConfiguration
    {
        /// <summary>
        /// Gets or sets the HTTP endpoint URL of the Ollama API.
        /// </summary>
        public string OllamaEndpoint { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the model to be used with the Ollama API.
        /// </summary>
        public string OllamaModelName { get; set; } = null!;
    }

}
