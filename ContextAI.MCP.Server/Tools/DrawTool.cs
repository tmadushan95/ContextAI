using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ContextAI.MCP.Server.Tools
{
    [McpServerToolType]
    public static class DrawTool
    {
        [McpServerTool, Description("Draws a line on a whiteboard.")]
        public static async Task DrawLine()
        {
            Console.WriteLine("Drawing a line on the whiteboard...");
        }
    }


}
