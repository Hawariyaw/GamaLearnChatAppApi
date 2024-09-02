namespace Server.Domain.Interface.Message
{
    public interface IMessageRepository
    {
        public Task<Entity.Message> CreateMessage(Entity.Message message);
        Task<Entity.Message?> UpdateMessage(Entity.Message message);
        public Task<Entity.Message?> GetMessage(Guid Id); 
        public Task<IEnumerable<Entity.Message>> GetFromMessages(string FromId); 
        public Task<IEnumerable<Entity.Message>> GetToMessages(string ToId); 
        public Task<IEnumerable<Entity.Message>> GetMessages(string ToId, string FromId); 
        public Task<IEnumerable<Entity.Message>> GetFailedMessages(int batchSize);

    }
}