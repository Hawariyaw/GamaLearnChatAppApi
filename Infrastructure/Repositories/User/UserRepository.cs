
using Microsoft.EntityFrameworkCore;
using Server.Domain.Interface.User;

namespace Server.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatDbContext _context;
        public UserRepository(ChatDbContext context)
        {
            _context = context;
        }
        public async Task<Entity.User> CreateUser(Entity.User user)
        {
            //hash password
            user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;
            user.Password = user.MakePasswordHash(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Entity.User?> GetUser(Guid Id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<Entity.User?> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<Entity.User?>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> VerifyUser(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return false;
            }
            return user.VerifyPassword(password);
        }
    }
}