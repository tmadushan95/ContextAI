using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextAI.MCP.Server.Tools
{
    /// <summary>
    /// A server-side tool that provides basic arithmetic operations, designed to be invoked by the MCP client framework.
    /// </summary>
    [McpServerToolType]
    public static class SumTool
    {
        /// <summary>
        /// Calculates the sum of two integers.
        /// This method can be invoked remotely by the MCP client as a server-side tool operation.
        /// </summary>
        /// <param name="a">The first integer value.</param>
        /// <param name="b">The second integer value.</param>
        /// <returns>A string containing the result of the addition.</returns>
        [McpServerTool]
        [Description("Calculates the sum of two integers.")]
        public static string SumTwoDigits(int a, int b)
        {
            return $"Sum of {a} and {b} is {a + b}";
        }
    }

}
