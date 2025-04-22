namespace Novator.Models.DTO
{
    public class CreateTankerShipRequest
    {
        public string IMO { get; set; }
        public string Name { get; set; }
        public double Length { get; set; } // in meters
        public double Beam { get; set; } // in meters
        public List<CreateTankRequest> Tanks { get; set; }
    }
}
