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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data here
            var user = new User { Id = Guid.NewGuid(), UserName = "admin", FirstName = "Hawariyaw", LastName = "Pawulos", Password = "" };
            user.Password = user.MakePasswordHash("admin");
            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<GroupConnection>().HasData(
                new GroupConnection { Id = Guid.NewGuid(), GroupName = "Learning Session 1", UserIds = new List<Guid> { user.Id } },
                new GroupConnection { Id = Guid.NewGuid(), GroupName = "Learning Session 2", UserIds = new List<Guid> { user.Id } },
                new GroupConnection { Id = Guid.NewGuid(), GroupName = "Learning Session 3", UserIds = new List<Guid> { user.Id } }
            );
        }
        
    }
}