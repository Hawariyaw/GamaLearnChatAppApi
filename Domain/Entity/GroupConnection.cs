namespace Server.Entity
{
    public class GroupConnection
    {
        public Guid Id { get; set; }
        public List<Guid>? UserIds { get; set; }
        public required string GroupName { get; set; }
    }
}