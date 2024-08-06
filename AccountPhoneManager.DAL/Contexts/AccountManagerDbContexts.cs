using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountPhoneManager.DAL.Contexts
{
    public class AccountManagerDbContexts(DbContextOptions<AccountManagerDbContexts> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Phone> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(a => a.PhoneNumbers)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Phone>()
                .Property(p => p.Number).HasMaxLength(11);
        }
    }
}
