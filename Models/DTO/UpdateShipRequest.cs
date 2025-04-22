using System.ComponentModel.DataAnnotations;

namespace Novator.Models.DTO
{
    public class UpdateShipRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Length { get; set; } // in meters
        public double Beam { get; set; } // in meters
    }
}
