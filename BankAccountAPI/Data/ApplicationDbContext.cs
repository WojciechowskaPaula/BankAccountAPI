using BankAccountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccountAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    } 
}

    
    

