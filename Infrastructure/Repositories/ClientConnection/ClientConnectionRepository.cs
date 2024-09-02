using Microsoft.EntityFrameworkCore;
using Server.Domain.Interface.ClientConnection;

namespace Server.Infrastructure.Repositories.ClientConnection
{
    public class ClientConnectionRepository: IClientConnectionRepository
    {
        private readonly ChatDbContext _context;
        public ClientConnectionRepository( ChatDbContext context)
        {
            _context = context;
        }

        public async Task<Entity.ClientConnection> CreateConnection(Entity.ClientConnection connection)
        {
            connection.Id = connection.Id == Guid.Empty ? Guid.NewGuid() : connection.Id;
            await _context.Connections.AddAsync(connection);
            await _context.SaveChangesAsync();
            return connection;
        }

        public async Task<Entity.ClientConnection?> GetConnection(Guid Id)
        {
            return await _context.Connections.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<Entity.ClientConnection?> GetConnectionByConnectionId(string connectionId)
        {
            return await _context.Connections.FirstOrDefaultAsync(c => c.ConnectionId == connectionId);
        }

        public async Task<Entity.ClientConnection?> GetConnectionByUserName(string UserName)
        {
            return await _context.Connections.FirstOrDefaultAsync(c => c.UserName == UserName);
        }

        public async Task<List<Entity.ClientConnection>> GetConnections(Guid UserId)
        {
            return await _context.Connections.Where(c => c.UserId == UserId).ToListAsync();
        }

        public async Task<List<Entity.ClientConnection>> GetConnections()
        {
            return await _context.Connections.ToListAsync();
        }
    }
}