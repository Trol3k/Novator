using System.ComponentModel.DataAnnotations;

namespace Novator.Models
{
    public abstract class Ship
    {
        [Key]
        [MaxLength(7)]
        public string IMO { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Length { get; set; } // in meters
        public double Beam { get; set; } // in meters
    }
}
