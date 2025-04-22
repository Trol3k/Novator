namespace Novator.Models
{
    public class PassengerShip:Ship
    {
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
    }
}
