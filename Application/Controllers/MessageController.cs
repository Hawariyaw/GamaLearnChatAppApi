using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Dto.Request;
using Server.Domain.Interface.ClientConnection;
using Server.Domain.Interface.Message;
using Server.Entity;
using Server.Infrastructure.Hubs;

namespace Server.Infrastructure.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "JwtScheme")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;

    private readonly ILogger<ChatHub> _chatHubLogger;

    private readonly IMessageRepository _messageRepository;

    private readonly IClientConnectionRepository _clientConnectionRepository;

    private readonly ChatHub _chatHub;

    private readonly Random _random = new Random();

    public MessageController(ILogger<MessageController> logger, ILogger<ChatHub> chatHubLogger, IMessageRepository messageRepository, IClientConnectionRepository clientConnectionRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
        _chatHubLogger = chatHubLogger;
        _clientConnectionRepository = clientConnectionRepository;
        _chatHub = new ChatHub(_chatHubLogger, _clientConnectionRepository, _messageRepository);
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

    [HttpGet("Message/CheckFailedMessagesAndResent")]
    public async Task<IActionResult> CheckFailedMessagesAndResent()
    {
        try
        {
            var batchSize = _random.Next(15, 35);
            var messages = await _messageRepository.GetFailedMessages(batchSize);
            foreach (var message in messages)
            {

                // send message again
                // if success update message.Delivered = true;
                // else leave it as it is
                await _chatHub.SendMessage(new MessageDto
                {
                    UserName = message.FromId,
                    ChatRoom = message.ToId,
                    Content = message.Content,
                    IsPrivate = message.MessageType == MessageType.Personal
                });

                var msg = await _messageRepository.GetMessage(message.Id);

                if (msg != null && msg.Delivered == true)
                {
                    message.Delivered = true;
                    await _messageRepository.UpdateMessage(message);
                }
            }
            return Ok("CheckFailedMessagesAndResent done!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CheckFailedMessagesAndResent");
            return Ok("Error in CheckFailedMessagesAndResent");
        }
    }

    [HttpGet("Message/AddJobs")]
    public IActionResult AddJobs()
    {
        using (var connection = JobStorage.Current.GetConnection())
        {
            foreach (var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection))
            {
                RecurringJob.RemoveIfExists(recurringJob.Id);
            }
        }
        _logger.LogInformation("**********************   Adding or Updating Hangfire jobs... ***********************");
        RecurringJob.AddOrUpdate("CheckFailedMessagesAndResent", () => CheckFailedMessagesAndResent(), "*/10 * * * *"); // every 10 minute
        return Ok("Jobs done!");
    }
}
