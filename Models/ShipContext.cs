using Microsoft.EntityFrameworkCore;

namespace Novator.Models
{
    public class ShipContext : DbContext
    {
        public ShipContext(DbContextOptions<ShipContext> options) : base(options) {}
        public DbSet<Ship> Ships { get; set; }
        public DbSet<PassengerShip> PassengerShips { get; set; }
        public DbSet<TankerShip> TankerShips { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ship>()
            .HasIndex(s => s.IMO)
            .IsUnique();

            modelBuilder.Entity<Ship>()
           .HasDiscriminator<string>("ShipType")
           .HasValue<PassengerShip>("Passenger")
           .HasValue<TankerShip>("Tanker");

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.PassengerShip)
                .WithMany(ps => ps.Passengers)
                .HasForeignKey(p => p.PassengerShipIMO)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tank>()
                .HasOne(t => t.TankerShip)
                .WithMany(ts => ts.Tanks)
                .HasForeignKey(t => t.TankerShipIMO)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
