using Microsoft.EntityFrameworkCore;
using Server.Domain.Interface.Group;

namespace Server.Infrastructure.Repositories.Group
{
    public class GroupRepository: IGroupRepository
    {
        private readonly ChatDbContext _context;
        public GroupRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<Entity.GroupConnection> CreateGroup(Entity.GroupConnection group)
        {
            group.Id = group.Id == Guid.Empty ? Guid.NewGuid() : group.Id;
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Entity.GroupConnection?> GetGroup(Guid Id)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == Id);
        }

        public async Task<List<Entity.GroupConnection>> GetGroups(Guid UserId)
        {
            return await _context.Groups.Where(g => g.UserIds!.Contains(UserId)).ToListAsync();
        }

        public async Task<List<Entity.GroupConnection>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }
    }
}