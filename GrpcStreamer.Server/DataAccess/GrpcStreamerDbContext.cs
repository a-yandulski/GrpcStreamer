using System;
using GrpcStreamer.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrpcStreamer.Server.DataAccess
{
    public class GrpcStreamerDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public GrpcStreamerDbContext(DbContextOptions<GrpcStreamerDbContext> options)
            :base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().ToTable("TItem");
            modelBuilder.Entity<Item>().HasKey(x => x.ItemId).HasName("PK_TItem_ItemId");
            modelBuilder.Entity<Item>().Property(x => x.ItemId).HasColumnName("ItemId").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Value).HasMaxLength(256);
            modelBuilder.Entity<Item>().Property(x => x.StatusId).HasColumnName("StatusId").IsRequired();
        }
    }
}
