using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;


// This is the entry point of the application.  
var service = new ServiceCollection();

// Configure the services for the Model Context Protocol (MCP) server.
service.AddMcpServer()
                    .WithStdioServerTransport()
                    .WithToolsFromAssembly();

// Build the service provider to create the service container.
var serviceProvider = service.BuildServiceProvider();

var mcpServer = serviceProvider.GetRequiredService<IMcpServer>();
await mcpServer.RunAsync();
