using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Novator.Models
{
    public class Tank
    {
        public int Id { get; set; }
        public FuelType? FuelType { get; set; }
        public double Capacity { get; set; } // in liters

        public double CurrentVolume { get; set; } // in liters

        [JsonIgnore]
        public string TankerShipIMO { get; set; }
        [JsonIgnore]
        public TankerShip TankerShip { get; set; }
    }
}
