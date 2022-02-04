using Microsoft.EntityFrameworkCore;
using NotezApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotezApp.Data.Contexts
{
    public class NotezContext : DbContext
    {
        public NotezContext(DbContextOptions<NotezContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotezContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .EnableServiceProviderCaching();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
