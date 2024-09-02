
namespace Server.Dto.Response
{
    public class UserDto
    {
        public  Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public UserDto(Entity.User? user)
        {
            if (user == null)
            {
                return;
            }
            Id = user.Id;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}