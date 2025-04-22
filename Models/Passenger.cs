using System.Text.Json.Serialization;

namespace Novator.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        [JsonIgnore]
        public string PassengerShipIMO { get; set; }
        [JsonIgnore]
        public PassengerShip PassengerShip { get; set; }
    }
}
