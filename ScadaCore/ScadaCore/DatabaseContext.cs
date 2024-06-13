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
    }
}