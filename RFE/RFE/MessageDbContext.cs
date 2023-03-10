using Microsoft.EntityFrameworkCore;

namespace RFE
{
    public class MessageDbContext: DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options)
            : base(options) { }

        public DbSet<Message> Messages => Set<Message>();
    }
}
