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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=your_server;Database=your_database;User Id=your_username;Password=your_password;");
        }
    }
}
