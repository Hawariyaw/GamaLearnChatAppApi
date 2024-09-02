using Microsoft.EntityFrameworkCore;
using Server.Entity;

namespace Server.Infrastructure
{
    public class ChatDbContext: DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<GroupConnection> Groups { get; set; }
        public virtual DbSet<ClientConnection> Connections { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }
        
    }
}