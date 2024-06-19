using ScadaCore.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScadaCore
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<AlarmEntity> alarms { get; set; }
        public DbSet<TagEntity> tags { get; set; }

        public DatabaseContext() : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<AlarmEntity>().HasIndex(a => a.Id).IsUnique();
            modelBuilder.Entity<TagEntity>().HasIndex(t => t.Id).IsUnique();
        }
    }
}