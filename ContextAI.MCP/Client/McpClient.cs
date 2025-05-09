using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

namespace ContextAI.MCP.Client
{

    public static class McpClient
    {

        public static async Task CreateMcpClientAsync()
        {
            try
            {
                var clientTransport = new StdioClientTransport(new()
                {
                    Name = "Demo Server",
                    Command = "dotnet",
                    Arguments = ["run", "--project", "D:\\Project\\ContextAI\\ContextAI\\ContextAI\\ContextAI.MCP\\ContextAI.MCP.csproj"],
                });

                await using var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

                // print list of tools available in server
                var tools = await mcpClient.ListToolsAsync();
                foreach (var tool in tools)
                {
                    Console.WriteLine($"Connected to server with tools: {tool.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating MCP client: {ex.Message}");
                throw;
            }
        }
    }
}
