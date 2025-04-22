using System.ComponentModel.DataAnnotations;

namespace Novator.Models.DTO
{
    public class RefuelTankRequest
    {
        public FuelType FuelType { get; set; }

        public double Volume { get; set; }
    }
}
