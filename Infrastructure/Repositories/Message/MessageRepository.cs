
using Microsoft.EntityFrameworkCore;
using Server.Domain.Interface.Message;

namespace Server.Infrastructure.Repositories.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatDbContext _context;
        public MessageRepository(ChatDbContext context)
        {
            _context = context;
        }
        public async Task<Entity.Message> CreateMessage(Entity.Message message)
        {
            message.Id = message.Id == Guid.Empty ? Guid.NewGuid() : message.Id;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;   
        }

        public async Task<Entity.Message?> UpdateMessage(Entity.Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
            return message;
        }
        
        public async Task<Entity.Message?> GetMessage(Guid Id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<IEnumerable<Entity.Message>> GetMessages(string ToId, string FromId)
        {
            return await _context.Messages.Where(m => (m.FromId == FromId && m.ToId == ToId) || (m.FromId == ToId && m.ToId == FromId)).ToListAsync();
        }

        public async Task<IEnumerable<Entity.Message>> GetToMessages(string ToId)
        {
            return await _context.Messages.Where(m => m.ToId == ToId).ToListAsync();
        }

        public async Task<IEnumerable<Entity.Message>> GetFromMessages(string FromId)
        {
            return await _context.Messages.Where(m => m.FromId == FromId).ToListAsync();
        }

        public async Task<IEnumerable<Entity.Message>> GetFailedMessages(int batchSize)
        {
            return await _context.Messages.Where(m => m.Delivered == false).Take(batchSize).ToListAsync();
        }

    }
}