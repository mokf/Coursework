using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        //builder.Entity<RentedApartments>().HasKey(key => new { key.AppUserId, key.ApartmentId });

        builder.Entity<AppUser>().HasMany(x => x.RentedApartments).WithOne(x => x.RentedByUser)
            .HasForeignKey(x => x.RentedByUserId);

        // builder.Entity<UserRent>().HasOne(source => source.SourceUser).WithMany(rented => rented.RentedApartments)
        //     .HasForeignKey(source => source.SourceUserId).OnDelete(DeleteBehavior.Cascade);
        //
        // builder.Entity<UserRent>().HasOne(target => target.TargetApartment).WithMany(rented => rented.RentedByUser)
        //     .HasForeignKey(target => target.TargetApartmentId).OnDelete(DeleteBehavior.Cascade);
    }
}