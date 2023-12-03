using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTrackerEfCore {
    internal class AssetContext : DbContext {
        public DbSet<Asset> Assets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=AssetDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Asset>().Property(a => a.Price).HasPrecision(18, 2); // Adjust precision and scale according to your requirements

            modelBuilder.Entity<Asset>()
                .Property(a => a.Modifier)
                .HasPrecision(18, 2); // Adjust precision and scale according to your requirements for the Modifier property
        }
    }
}
