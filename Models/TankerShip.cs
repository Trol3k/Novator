namespace Novator.Models
{
    public class TankerShip:Ship
    {
       public List<Tank> Tanks { get; set; } = new List<Tank>();
    }
}
