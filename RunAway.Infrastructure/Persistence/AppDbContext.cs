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
        public DbSet<RoomAvailableRecordEntity> RoomAvailableRecords { get; set; } = null!;
        public DbSet<TransactionRecordEntity> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Accommodation entity
            modelBuilder.Entity<AccommodationEntity>(entity =>
            {
                entity.ToTable("accommodations");
                entity.HasKey(e => e.Id).HasName("pk_accommodations");

                // Property column name configuration
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.ImageUrls).HasColumnName("image_urls");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");

                // Room relationship configuration
                entity.HasMany(e => e.Rooms)
                    .WithOne(r => r.Accommodation)
                    .HasForeignKey(r => r.AccommodationId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Tell EF to use the private field for the navigation
                entity.Navigation(e => e.Rooms).UsePropertyAccessMode(PropertyAccessMode.Field);

                // Configure Coordinate value object
                entity.OwnsOne(e => e.Coordinate, coordinate =>
                {
                    coordinate.Property(c => c.Latitude).HasColumnName("latitude");
                    coordinate.Property(c => c.Longitude).HasColumnName("longitude");
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
                entity.ToTable("rooms");
                entity.HasKey(e => e.Id).HasName("pk_rooms");

                // Property column name configuration
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");

                // Configure Money value object
                entity.OwnsOne(e => e.Price, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("price");
                    price.Property(p => p.Currency).HasColumnName("currency");
                });

                // Configure collection of facilities
                entity.Property(e => e.Facilities)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                    .HasColumnName("facilities");
            });

            // Configure User entity
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id).HasName("pk_users");

                // Property column name configuration
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");

                // Configure the Email value object
                entity.OwnsOne(e => e.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("email_address")
                         .HasMaxLength(320);

                    email.HasIndex(e => e.Value).IsUnique();

                    // Tell EF Core about the shadow property (foreign key)
                    email.WithOwner()
                         .HasForeignKey("user_id");
                });

                // Configure the Transaction Entity 
                entity.HasMany(u => u.Transactions)
                  .WithOne(t => t.User)
                  .HasForeignKey(t => t.UserID)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Transaction entity
            modelBuilder.Entity<TransactionRecordEntity>(entity =>
            {
                entity.ToTable("transaction_records");
                entity.HasKey(e => e.Id).HasName("pk_transactions_records");

                // Property column name configuration
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RoomID).HasColumnName("room_id");
                entity.Property(e => e.UserID).HasColumnName("user_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");
                entity.Property(e => e.TransactionStatus).HasColumnName("transaction_status");

                entity.OwnsOne(e => e.Amount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("amount");
                    money.Property(m => m.Currency).HasColumnName("currency");
                });

                entity.HasOne(t => t.Room)
                    .WithMany()
                    .HasForeignKey(t => t.RoomID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.OwnsMany(p => p.Guests, a =>
                 {
                     a.ToTable("transaction_guests");
                     a.WithOwner().HasForeignKey("transaction_record_id");

                     // Define the shadow property for the primary key
                     a.Property<int>("id").ValueGeneratedOnAdd().HasColumnName("id");

                     // Set it as the key
                     a.HasKey("id").HasName("pk_transaction_guests");

                     // Property column name configuration
                     a.Property(e => e.Type).HasColumnName("type");
                     a.Property(e => e.Number).HasColumnName("number");
                     entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                     entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");
                 });
            });

            // Configure Room Available entity
            modelBuilder.Entity<RoomAvailableRecordEntity>(entity =>
            {
                entity.ToTable("room_available_records");
                entity.HasKey(e => e.Id).HasName("pk_room_available_records");

                // Property column name configuration
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.AvailableRooms).HasColumnName("available_rooms");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastUpdatedAt).HasColumnName("last_updated_at");

                entity.Property(e => e.Date)
                .HasConversion(
                    dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                    dateTime => DateOnly.FromDateTime(dateTime)
                );

                entity.HasIndex(a => new { a.RoomId, a.Date })
                    .IsUnique();

                entity.HasOne(a => a.Room)
                 .WithMany(r => r.RoomAvailabilityRecords)
                 .HasForeignKey(a => a.RoomId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
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