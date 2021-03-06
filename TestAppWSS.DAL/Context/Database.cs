using Microsoft.EntityFrameworkCore;
using TestAppWSS.Domain.Entities;

namespace TestAppWSS.DAL
{
    public class Database : DbContext
    {
        public DbSet<Node> Departments { get; set; }


        public Database(DbContextOptions<Database> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder db)
        {
            db.Entity<Node>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Children)
                    .WithOne(x => x.Parent)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}