using ContextAI.MCP.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

//namespace ContextAI.MCP.Server.Server
//{
//    internal class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var builder = Host.CreateApplicationBuilder(args);

//            // Add services to the container.
//            builder.Services.AddMCPServices();

//            // Build the host
//            var app = builder.Build();


//            // run MCP  client
//            //McpClient mcpClient = app.Services.GetRequiredService<McpClient>();
//            //await mcpClient.CreateMcpClientAsync();


//            // Run MCP client
//            await app.RunAsync(); // Adjusted for synchronous Main method
//        }
//    }
//}

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container.
builder.Services.AddMCPServices();

// Build the host
var app = builder.Build();


// run MCP  client
//McpClient mcpClient = app.Services.GetRequiredService<McpClient>();
//await mcpClient.CreateMcpClientAsync();


// Run MCP client
await app.RunAsync(); // Adjusted for synchronous Main method
