using ContextAI.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ContextAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class McpClientController(IMcpClientService clientService) : ControllerBase
    {
        private readonly IMcpClientService _clientService = clientService;

        [HttpGet("index")]
        public Task<List<string>> GetActionResult(string userInput)
        {
            return _clientService.GenerateAssistantResponseAsync(userInput);
        }
    }
}
