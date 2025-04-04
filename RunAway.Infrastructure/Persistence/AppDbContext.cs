using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RunAway.Domain.Entities;


namespace RunAway.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AccommodationEntity> Accommodations { get; set; } = null!;
        public DbSet<RoomEntity> Rooms { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccommodationEntity>(entity =>
            {
                entity.OwnsOne(a => a.Coordinate, coord =>
                {
                    coord.Property(c => c.Latitude).HasColumnName("Latitude");
                    coord.Property(c => c.Longitude).HasColumnName("Longitude");
                });

                entity
                    .Property<List<string>>("_imageUrls")
                    .HasColumnName("ImageUrls")
                    .HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .Metadata.SetValueComparer(
                        new ValueComparer<List<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                        ));

                entity.HasMany(a => a.Rooms)
                    .WithOne(r => r.Accommodation)
                    .HasForeignKey(r => r.AccommodationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RoomEntity>(entity =>
            {
                entity.OwnsOne(r => r.Price, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("Amount");
                    money.Property(m => m.Currency).HasColumnName("Currency");
                });
            });
        }
    }
}
