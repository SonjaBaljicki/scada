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
    }
}