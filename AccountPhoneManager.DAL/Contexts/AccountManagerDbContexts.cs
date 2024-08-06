using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.DAL.Contexts
{
    public class AccountManagerDbContexts(DbContextOptions<AccountManagerDbContexts> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Phone> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional model configurations can go here
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue("Active");
            });
        }
    }
}
