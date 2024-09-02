namespace Server.Domain.Interface.Group
{
    public interface IGroupRepository
    {
        public Task<Entity.GroupConnection> CreateGroup(Entity.GroupConnection group);
        public Task<Entity.GroupConnection?> GetGroup(Guid Id);
        public Task<List<Entity.GroupConnection>> GetGroups(Guid UserId);
        public Task<List<Entity.GroupConnection>> GetGroups();
    }
}