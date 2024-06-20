    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proiect.Models;

namespace proiect.Data
{
    public class proiectContext : DbContext
    {
        public proiectContext (DbContextOptions<proiectContext> options)
            : base(options)
        {
        }

        public DbSet<proiect.Models.Movie> Movie { get; set; } = default!;

        public DbSet<proiect.Models.Producer>? Producer { get; set; }

        public DbSet<proiect.Models.Director>? Director { get; set; }

        public DbSet<proiect.Models.Genre>? Genre { get; set; }

        public DbSet<proiect.Models.Member>? Member { get; set; }

        public DbSet<proiect.Models.Borrowing>? Borrowing { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(b => b.Borrowing)
                .WithOne(b => b.Movie)
                .HasForeignKey<Borrowing>(b => b.MovieID)
                .HasPrincipalKey<Movie>(b => b.ID);
        }
    }
}
