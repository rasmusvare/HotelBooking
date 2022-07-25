using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    
    public DbSet<Amenity> Amenities { get; set; } = default!;
    public DbSet<Booking> Bookings { get; set; } = default!;
    public DbSet<Guest> Guests { get; set; } = default!;
    public DbSet<Hotel> Hotels { get; set; } = default!;
    public DbSet<Room> Rooms { get; set; } = default!;
    public DbSet<RoomType> RoomTypes { get; set; } = default!;
    public DbSet<Stay> Stays { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Remove cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        builder.Entity<Booking>()
            .HasMany(x => x.Guests)
            .WithOne(x => x.Booking)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<RoomType>()
            .HasMany(x => x.Amenities)
            .WithMany(x => x.RoomTypes)
            .UsingEntity<Dictionary<string, object>>(
                "AmenityRoomType",
                j => j.HasOne<Amenity>().WithMany().OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<RoomType>().WithMany().OnDelete(DeleteBehavior.Cascade));
        
        builder.Entity<RoomType>()
            .HasMany(x => x.Rooms)
            .WithOne(x => x.RoomType!)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<RoomType>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.RoomType)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {
        FixEntities(this);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void FixEntities(AppDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
    }
}