using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Interface.Message;
using Server.Entity;

namespace Server.Infrastructure.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "JwtScheme")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;

    private readonly IMessageRepository _messageRepository;

    public MessageController(ILogger<MessageController> logger, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
    }

    [HttpPost("Message")]
    public async Task<Message> Create(Message message)
    {
        return await _messageRepository.CreateMessage(message);
    }

    [HttpGet("Messages/{ToId}/{FromId}")]
    public async Task<IEnumerable<Message?>> GetAll(string FromId, string ToId)
    {
        return await _messageRepository.GetMessages(ToId, FromId);
    }

    [HttpGet("MessagesTo/{ToId}")]
    public async Task<IEnumerable<Message?>> GetByTo(string ToId)
    {
        return await _messageRepository.GetToMessages(ToId);
    }

    [HttpGet("MessagesFrom/{FromId}")]
    public async Task<IEnumerable<Message?>> GetByFrom(string FromId)
    {
        return await _messageRepository.GetFromMessages(FromId);
    }

    [HttpGet("Message/{Id}")]
    public async Task<Message?> Get(Guid Id)
    {
        return await _messageRepository.GetMessage(Id);
    }
}
