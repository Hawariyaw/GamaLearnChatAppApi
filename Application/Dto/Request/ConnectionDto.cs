namespace Server.Dto.Request
{
    public class ConnectionDto
    {
        public Guid UserId { get; set; }
        public string? ConnectionId { get; set; }
        public required string UserName { get; set; }
        public required string ChatRoom { get; set; }
        public string? PrevChatRoom { get; set; }
    }
}