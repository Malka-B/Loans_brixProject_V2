using Loans.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loans.Data
{
    public class LoansContext : DbContext
    {
        public LoansContext(DbContextOptions<LoansContext> options) : base(options)
        {

        }

        public LoansContext()
        { }

        public DbSet<LoanEntity> Loans { get; set; }
        public DbSet<LoanFailureRulesEntity> FailureRules { get; set; }
              
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanEntity>()
                .HasMany(l => l.FailureRules)
                .WithOne(e => e.Loan);
        }
    }
}
