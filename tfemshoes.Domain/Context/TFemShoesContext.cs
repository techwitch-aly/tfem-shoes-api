using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Entities;

namespace tfemshoes.Domain.Context
{
    public class TFemShoesContext : DbContext
    {
        #region DbSets
        public DbSet<Store> Stores { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        public TFemShoesContext(DbContextOptions<TFemShoesContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(s =>
            {
                s.ToTable("Store");
                s.Property(p => p.Latitude).HasPrecision(8, 6);
                s.Property(p => p.Longitude).HasPrecision(9, 6);
            });

            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
