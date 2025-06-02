using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextAI.Infrastructure.Tools
{
    [McpServerToolType]
    public static class SumTool
    {
        [McpServerTool, Description("Sums two digits.")]
        public static string SumTwoDigits(int a, int b)
        {
            return $"Sum of {a} and {b} is {a + b}";
        }
    }
}
