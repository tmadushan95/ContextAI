using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextAI.MCP.Server.Tools
{

    /// <summary>
    /// Represents a tool for drawing on a whiteboard.
    /// </summary>
    [McpServerToolType]
    public static class DrawTool
    {
        // [McpServerTool] attribute is used to mark methods as tools that can be called by the MCP client.
        [McpServerTool, Description("Draws a line on a whiteboard.")]
        public static string DrawLine()
        {
            return "Drawing a line on the whiteboard...";
        }
    }


}
