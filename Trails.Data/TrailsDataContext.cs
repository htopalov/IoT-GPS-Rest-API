using Microsoft.EntityFrameworkCore;
using Trails.Domain.Models;

namespace Trails.Data
{
    public class TrailsDataContext : DbContext
    {
        public TrailsDataContext(DbContextOptions<TrailsDataContext> options)
            :base()
        {
            
        }

        public virtual DbSet<Device> Devices { get; set; }

        public virtual DbSet<PositionData> PositionData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PositionData>()
                .HasOne(x => x.Device)
                .WithMany(x => x.PositionData)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
