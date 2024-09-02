namespace Server.Entity
{
    public class ClientConnection
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? ConnectionId { get; set; }
        public required string UserName { get; set; }
        public required string ChatRoom { get; set; }
    }
}