using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextAI.MCP.Server.Tools
{
    /// <summary>
    /// Represents a tool for summing two digits.
    /// </summary>
    [McpServerToolType]
    public static class SumTool
    {
        // [McpServerTool] attribute is used to mark methods as tools that can be called by the MCP client.
        [McpServerTool, Description("Sums two digits.")]
        public static string SumTwoDigits(int a, int b)
        {
            return $"Sum of {a} and {b} is {a + b}";
        }
    }
}
