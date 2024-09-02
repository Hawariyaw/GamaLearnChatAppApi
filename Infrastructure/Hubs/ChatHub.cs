using Microsoft.AspNetCore.SignalR;
using Server.Application.Dto.Request;
using Server.Domain.Interface.ClientConnection;
using Server.Domain.Interface.Message;
using Server.Dto.Request;
using Server.Entity;

namespace Server.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IClientConnectionRepository _clientConnectionRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatHub(ILogger<ChatHub> logger, IClientConnectionRepository clientConnectionRepository, IMessageRepository messageRepository)
        {
            _logger = logger;
            _clientConnectionRepository = clientConnectionRepository;
            _messageRepository = messageRepository;
        }

        public async Task JoinChat(ClientConnection userConnection)
        {
            try
            {
                await Clients.All.SendAsync("JoinChat", userConnection.UserName, $"{userConnection.ChatRoom} has joined.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinChat");
            }
        }

        public async Task JoinSpecificChatRoom(ConnectionDto userConnection)
        {
            try
            {
                //check if user is already in a chat room
                var connection = await _clientConnectionRepository.GetConnectionByUserName(userConnection.UserName);
                if(connection == null)
                {
                    //add user to chat room
                    connection = await _clientConnectionRepository.CreateConnection(new ClientConnection
                    {
                        Id = new Guid(),
                        UserId = userConnection.UserId,
                        ConnectionId = userConnection.ConnectionId,
                        UserName = userConnection.UserName,
                        ChatRoom = userConnection.ChatRoom
                    });

                }
                //clear existing connection before add
                if(!string.IsNullOrEmpty(userConnection.PrevChatRoom))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.PrevChatRoom);
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);
                await Clients.Group(userConnection.ChatRoom).SendAsync("JoinSpecificChatRoom", userConnection.UserName, $"{userConnection.UserName} has joined {userConnection.ChatRoom}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinSpecificChatRoom for user {UserName} in room {ChatRoom}", userConnection.UserName, userConnection.ChatRoom);
            }
        }

        public async Task SendMessage(MessageDto Message)
        {
            var connection = await _clientConnectionRepository.GetConnectionByUserName(Message.UserName);
            if(connection != null)
            {   
                if(!Message.IsPrivate)
                    await Clients.Group(Message.ChatRoom).SendAsync("ReceiveSpecificChatRoom", Message.UserName, Message.Content);
                else
                {
                    await Clients.Group(Message.UserName).SendAsync("ReceiveSpecificChatRoom", Message.UserName, Message.Content);
                    await Clients.Group(Message.ChatRoom).SendAsync("ReceiveSpecificChatRoom", Message.UserName, Message.Content);
                    await Clients.Group($"{Message.ChatRoom.Split('_')[1]}_{Message.ChatRoom.Split('_')[0]}").SendAsync("ReceiveSpecificChatRoom", Message.UserName, Message.Content);
                }
                //save message to database
                _ = _messageRepository.CreateMessage(new Message
                    {
                        Id = Guid.NewGuid(),
                        MessageType = MessageType.Personal,
                        FromId = Message.UserName,
                        ToId = Message.ToUserName,
                        Content = Message.Content,
                        SentAt = DateTime.Now,
                        Delivered = true
                    });
            }
        }
    }
}