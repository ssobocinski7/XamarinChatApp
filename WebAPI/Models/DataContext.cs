using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FriendLink> FriendLinks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=tcp:xamarin.database.windows.net,1433;" +
                    "Initial Catalog=xamarinDatabase;" +
                    "Persist Security Info=False;" +
                    "User ID=xamarin;Password=!Test123;" +
                    "MultipleActiveResultSets=False;" +
                    "Encrypt=True;" +
                    "TrustServerCertificate=False;" +
                    "Connection Timeout=30;");
            }
        }

    }
}
