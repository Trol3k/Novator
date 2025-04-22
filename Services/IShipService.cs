using Novator.Models;
using Novator.Models.DTO;

namespace Novator.Services
{
    public interface IShipService
    {
        public Task<IEnumerable<Ship>> GetAllShipsAsync();
        public Task<Ship> GetShipByIMOAsync(string IMO);
        public Task AddShipAsync(Ship ship);
        public Task UpdateShipAsync(string IMO, UpdateShipRequest ship);
        public Task DeleteShipAsync(string IMO);
        public Task AddPassengerAsync(string IMO, Passenger passenger);
        public Task DeletePassengerAsync(string IMO, int passengerId);
        public Task RefuelTankAsync(string IMO, int tankId, FuelType fuelType, double volume);
        public Task EmptyTankAsync(string IMO, int tankId);
    }
}
