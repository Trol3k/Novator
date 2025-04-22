using Microsoft.AspNetCore.Mvc;
using Novator.Models;
using Novator.Models.DTO;
using Novator.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Novator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipController : ControllerBase
    {
        private readonly IShipService _shipService;

        public ShipController(IShipService shipService)
        {
            _shipService = shipService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ship>>> Get()
        {
            var ships = await _shipService.GetAllShipsAsync();

            return Ok(ships);
        }

        [HttpGet("{IMO}")]
        public async Task<ActionResult<Ship>> Get(string IMO)
        {
            try
            {
                var ship = await _shipService.GetShipByIMOAsync(IMO);
                return Ok(ship);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("passenger")]
        public async Task<ActionResult<PassengerShip>> Post([FromBody] CreatePassengerShipRequest ship)
        {
            try
            {
                if (ship == null)
                {
                    return BadRequest("Ship cannot be null");
                }

                var createdShip = new PassengerShip
                {
                    IMO = ship.IMO,
                    Name = ship.Name,
                    Type = "Passenger",
                    Length = ship.Length,
                    Beam = ship.Beam
                };

                await _shipService.AddShipAsync(createdShip);

                return CreatedAtAction(nameof(Get), new { IMO = createdShip.IMO }, createdShip);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("tanker")]
        public async Task<ActionResult<TankerShip>> Post([FromBody] CreateTankerShipRequest ship)
        {
            try
            {
                if (ship == null)
                {
                    return BadRequest("Ship cannot be null");
                }
                if (ship.Tanks == null || ship.Tanks.Count == 0)
                {
                    return BadRequest("Tanker ship must have at least one tank");
                }
                foreach (var tank in ship.Tanks)
                {
                    if (tank.Capacity <= 0)
                    {
                        return BadRequest("Tank capacity must be positive");
                    }
                }

                var createdShip = new TankerShip
                {
                    IMO = ship.IMO,
                    Name = ship.Name,
                    Type = "Tanker",
                    Length = ship.Length,
                    Beam = ship.Beam,
                    Tanks = ship.Tanks.Select(t => new Tank
                    {
                        Capacity = t.Capacity
                    }).ToList()
                };

                await _shipService.AddShipAsync(createdShip);

                return CreatedAtAction(nameof(Get), new { IMO = createdShip.IMO }, createdShip);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{IMO}")]
        public async Task<ActionResult<Ship>> Put(string IMO, [FromBody] UpdateShipRequest ship)
        {
            try
            {
                if (ship == null)
                {
                    return BadRequest("Ship cannot be null");
                }

                await _shipService.UpdateShipAsync(IMO, ship);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{IMO}")]
        public async Task<IActionResult> Delete(string IMO)
        {
            try
            {
                await _shipService.DeleteShipAsync(IMO);
                return NoContent();
            }
            catch (
                KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{IMO}/passengers")]
        public async Task<ActionResult<Passenger>> AddPassenger(string IMO, [FromBody] CreatePassengerRequest passenger)
        {
            try
            {
                var createdPassenger = new Passenger
                {
                    Name = passenger.Name,
                    Age = passenger.Age
                };

                await _shipService.AddPassengerAsync(IMO, createdPassenger);
                return CreatedAtAction(nameof(Get), new { IMO = IMO }, passenger);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{IMO}/passengers/{passengerId}")]
        public async Task<IActionResult> DeletePassenger(string IMO, [FromRoute] int passengerId)
        {
            try
            { 
                await _shipService.DeletePassengerAsync(IMO, passengerId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{IMO}/tanks/{tankId}/refuel")]
        public async Task<ActionResult<Tank>> RefuelTank(string IMO, int tankId, [FromBody] RefuelTankRequest request)
        {
            try
            {
                await _shipService.RefuelTankAsync(IMO, tankId, request.FuelType, request.Volume);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{IMO}/tanks/{tankId}/empty")]
        public async Task<IActionResult> EmptyTank(string IMO, int tankId)
        {
            try
            {
                await _shipService.EmptyTankAsync(IMO, tankId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
