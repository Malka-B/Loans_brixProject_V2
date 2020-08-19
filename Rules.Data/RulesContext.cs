using Microsoft.EntityFrameworkCore;
using Rules.Data.Entities;

namespace Rules.Data
{
    public class RulesContext : DbContext
    {
        public RulesContext(DbContextOptions<RulesContext> options) : base(options)
        {

        }

        public RulesContext()
        {

        }

        public DbSet<Rule> Rule { get; set; }
        public DbSet<RulesParameter> RulesParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rule>()
               .HasMany(c => c.ChildrenRules)
               .WithOne(c => c.ParentRule);

            modelBuilder.Entity<Rule>()
               .Property(r => r.Id)
               .ValueGeneratedOnAdd();
        }
    }
}
