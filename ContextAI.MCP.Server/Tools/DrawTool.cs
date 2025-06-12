using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextAI.MCP.Server.Tools
{

    /// <summary>
    /// Defines a whiteboard drawing tool that can be invoked via MCP server tooling infrastructure.
    /// </summary>
    [McpServerToolType]
    public static class DrawTool
    {
        /// <summary>
        /// Draws a line on the whiteboard surface.
        /// This method is exposed to the MCP client as an invocable server-side tool.
        /// </summary>
        /// <returns>A confirmation message indicating the line has been drawn.</returns>
        [McpServerTool]
        [Description("Draws a line on the whiteboard.")]
        public static string DrawLine()
        {
            return "Drawing a line on the whiteboard...";
        }
    }


}
