using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Trails.Models.Context
{
    public class TrailsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.CONNECTION_STRING);
            }
        }


        public virtual DbSet<Device> Devices { get; set; }

        public virtual DbSet<PositionData> PositionData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasMany(pd => pd.PositionData)
                .WithOne(d => d.Device)
                .HasForeignKey(k => k.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
