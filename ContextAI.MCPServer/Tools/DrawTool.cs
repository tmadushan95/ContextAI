using McpDotNet.Server;
using System.ComponentModel;

namespace ContextAI.MCPServer.Tools
{
    [McpToolType]
    public static class DrawTool
    {
        [McpTool, Description("Draws a line on a whiteboard.")]
        public static async Task DrawLine()
        {
            Console.WriteLine("Drawing a line on the whiteboard...");
        }
    }
}
