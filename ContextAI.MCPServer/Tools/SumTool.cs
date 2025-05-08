using McpDotNet.Server;
using System.ComponentModel;

namespace ContextAI.MCPServer.Tools
{
    [McpToolType]
    public static class SumTool
    {
        [McpTool, Description("Sums two digits.")]
        public static string SumTwoDigits(int a, int b)
        {
            return $"Sum of {a} and {b} is {a+b}";
        }
    }
}
