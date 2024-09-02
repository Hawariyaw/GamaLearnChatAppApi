using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Interface.ClientConnection;
using Server.Entity;

namespace Server.Infrastructure.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "JwtScheme")]
public class ClientConnectionController : ControllerBase
{
    private readonly ILogger<ClientConnectionController> _logger;

    private readonly IClientConnectionRepository _clientConnectionRepository;

    public ClientConnectionController(ILogger<ClientConnectionController> logger, IClientConnectionRepository clientConnectionRepository)
    {
        _logger = logger;
        _clientConnectionRepository = clientConnectionRepository;
    }

    [HttpPost("ClientConnection")]
    public async Task<ClientConnection> Create(ClientConnection connection)
    {
        return await _clientConnectionRepository.CreateConnection(connection);
    }

    [HttpGet("ClientConnections")]
    public async Task<IEnumerable<ClientConnection?>> GetAll()
    {
        return await _clientConnectionRepository.GetConnections();
    }

    [HttpGet("ClientConnections/{Id}")]
    public async Task<ClientConnection?> Get(Guid Id)
    {
        return await _clientConnectionRepository.GetConnection(Id);
    }
}
