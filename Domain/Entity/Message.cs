namespace Server.Entity
{
    public class Message
    {
        public Guid Id { get; set; }
        public required MessageType MessageType { get; set; }
        public required string FromId { get; set; }
        public required string ToId { get; set; }
        public required string Content { get; set; }
        public required DateTime SentAt { get; set; }
    }

    public enum MessageType
    {
       Personal,
       Group
    }
}