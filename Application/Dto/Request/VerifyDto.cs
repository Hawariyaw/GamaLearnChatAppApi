namespace Server.Dto.Request
{
    public class VerifyDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}