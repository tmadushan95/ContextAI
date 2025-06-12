using ModelContextProtocol.Client;

namespace ContextAI.Application.Interfaces.IServices
{

    public interface IMcpClientService
    {
        Task<List<string>> StartMcpClientAsync(string userInput);
    }
}
