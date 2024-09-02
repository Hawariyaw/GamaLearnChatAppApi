namespace Server.Application.Dto.Request
{
    public class MessageDto
    {
        public string UserName { get; set; }
        public string ChatRoom { get; set; }
        public bool IsPrivate { get; set; }
        public string Content { get; set; }
    }
}