namespace Server.Domain.Interface.User
{
    public interface IUserRepository
    {
        public Task<Entity.User> CreateUser(Entity.User user);
        public Task<Entity.User?> GetUser(Guid Id);
        public Task<Entity.User?> GetUserByUserName(string userName);
        Task<IEnumerable<Entity.User?>> GetUsers();
        Task<bool> VerifyUser(string userName, string password);
    }
}