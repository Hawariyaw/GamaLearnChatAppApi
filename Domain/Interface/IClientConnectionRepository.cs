namespace Server.Domain.Interface.ClientConnection
{
    public interface IClientConnectionRepository
    {
        public Task<Entity.ClientConnection> CreateConnection(Entity.ClientConnection connection);
        public Task<Entity.ClientConnection?> GetConnection(Guid Id);
        public Task<List<Entity.ClientConnection>> GetConnections(Guid UserId);
        public Task<List<Entity.ClientConnection>> GetConnections();
        Task<Entity.ClientConnection?> GetConnectionByConnectionId(string connectionId);
        Task<Entity.ClientConnection?> GetConnectionByUserName(string UserName);
    }
}