using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;


namespace RunAway.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AccommodationEntity> Accommodations { get; set; } = null!;
        public DbSet<RoomEntity> Rooms { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Accommodation entity
            modelBuilder.Entity<AccommodationEntity>(entity =>
            {
                entity.ToTable("Accommodations");
                entity.HasKey(e => e.Id);

                // Don't ignore — instead configure the backing field
                entity.HasMany(e => e.Rooms)
                    .WithOne(r => r.Accommodation)
                    .HasForeignKey(r => r.AccommodationId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Tell EF to use the private field for the navigation
                entity.Navigation(e => e.Rooms).UsePropertyAccessMode(PropertyAccessMode.Field);

                // Configure Coordinate value object
                entity.OwnsOne(e => e.Coordinate, coordinate =>
                {
                    coordinate.Property(c => c.Latitude).HasColumnName("Latitude");
                    coordinate.Property(c => c.Longitude).HasColumnName("Longitude");
                });

                // Configure collection of image URLs
                entity.Property(e => e.ImageUrls)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            // Configure Room entity
            modelBuilder.Entity<RoomEntity>(entity =>
            {
                entity.ToTable("Rooms");
                entity.HasKey(e => e.Id);

                // Configure Money value object
                entity.OwnsOne(e => e.Price, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("Price");
                    price.Property(p => p.Currency).HasColumnName("Currency");
                });

                // Configure collection of facilities
                entity.Property(e => e.Facilities)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                // Configure the Email value object
                entity.OwnsOne(e => e.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("EmailAddress")
                         .HasMaxLength(320);

                    email.HasIndex(e => e.Value).IsUnique();

                    // Tell EF Core about the shadow property (foreign key)
                    email.WithOwner()
                         .HasForeignKey("UserEntityId");
                });
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Apply audit information before saving
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditableEntity<Guid> auditableEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // Your AuditableEntity already sets CreatedAt in constructor
                            break;
                        case EntityState.Modified:
                            // Use your existing method
                            auditableEntity.SetUpdatedAt();
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}