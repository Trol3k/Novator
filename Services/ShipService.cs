using Microsoft.EntityFrameworkCore;
using Novator.Models;
using Novator.Models.DTO;
using Novator.Services.Validation;

namespace Novator.Services
{
    public class ShipService : IShipService
    {
        private readonly ShipContext _context;

        public ShipService(ShipContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ship>> GetAllShipsAsync()
        {
            return await _context.Ships.ToListAsync();
        }

        public async Task<Ship> GetShipByIMOAsync(string IMO)
        {
            var ship = await _context.Ships
                .Include(s => (s as TankerShip).Tanks)
                .Include(s => (s as PassengerShip).Passengers)
                .FirstOrDefaultAsync(s => s.IMO == IMO);

            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            return ship;
        }

        public async Task AddShipAsync(Ship ship)
        {
            if (_context.Ships.Any(s => s.IMO == ship.IMO))
                throw new ArgumentException($"Ship with IMO {ship.IMO} already exists");

            if (!IMOValidator.IsIMOValid(ship.IMO))
                throw new ArgumentException("Unvalid IMO number");

            if (ship.Length <= 0 || ship.Beam <= 0)
                throw new ArgumentException("Length and beam must be positive");

            await _context.Ships.AddAsync(ship);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShipAsync(string IMO, UpdateShipRequest updatedShip)
        {
            var ship = await _context.Ships.FirstOrDefaultAsync(s => s.IMO == IMO);
            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            ship.Name = updatedShip.Name;
            ship.Type = updatedShip.Type;
            ship.Length = updatedShip.Length;
            ship.Beam = updatedShip.Beam;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteShipAsync(string IMO)
        {
            var ship = await _context.Ships.FirstOrDefaultAsync(s => s.IMO == IMO);
            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            _context.Ships.Remove(ship);
            await _context.SaveChangesAsync();
        }

        public async Task AddPassengerAsync(string IMO, Passenger passenger)
        {
            var ship = await _context.Ships
                .OfType<PassengerShip>()
                .Include(ps => ps.Passengers)
                .FirstOrDefaultAsync(s => s.IMO == IMO);

            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            ship.Passengers.Add(passenger);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePassengerAsync(string IMO, int passenggerId)
        {
            var ship = await _context.Ships
                .OfType<PassengerShip>()
                .Include(ps => ps.Passengers)
                .FirstOrDefaultAsync(s => s.IMO == IMO);

            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            var existingPassenger = ship.Passengers.FirstOrDefault(p => p.Id == passenggerId);
            if (existingPassenger == null)
                throw new KeyNotFoundException($"Passenger with ID {passenggerId} not found on ship with IMO {IMO}");

            ship.Passengers.Remove(existingPassenger);
            await _context.SaveChangesAsync();
        }

        public async Task RefuelTankAsync(string IMO, int tankId, FuelType fuelType, double volume)
        {
            var ship = await _context.Ships
                .OfType<TankerShip>()
                .Include(ts => ts.Tanks)
                .FirstOrDefaultAsync(s => s.IMO == IMO);

            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            var tank = ship.Tanks.FirstOrDefault(t => t.Id == tankId);

            if (tank == null)
                throw new KeyNotFoundException($"Tank with ID {tankId} not found on ship with IMO {IMO}");

            if (tank.CurrentVolume + volume > tank.Capacity)
                throw new InvalidOperationException($"Tank with ID {tankId} on ship with IMO {IMO} cannot hold that much fuel");

            if (tank.FuelType != null && tank.FuelType != fuelType)
                throw new InvalidOperationException($"Tank with ID {tankId} on ship with IMO {IMO} already contains a different fuel type");

            tank.CurrentVolume += volume;
            tank.FuelType = fuelType;

            await _context.SaveChangesAsync();
        }

        public async Task EmptyTankAsync(string IMO, int tankId)
        {
            var ship = await _context.Ships
                .OfType<TankerShip>()
                .Include(ts => ts.Tanks)
                .FirstOrDefaultAsync(s => s.IMO == IMO);

            if (ship == null)
                throw new KeyNotFoundException($"Ship with IMO {IMO} not found");

            var tank = ship.Tanks.FirstOrDefault(t => t.Id == tankId);

            if (tank == null)
                throw new KeyNotFoundException($"Tank with ID {tankId} not found on ship with IMO {IMO}");

            tank.CurrentVolume = 0;
            tank.FuelType = null;

            await _context.SaveChangesAsync();
        }
    }
}
