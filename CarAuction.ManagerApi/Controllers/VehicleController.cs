using CarAuction.Model;
using CarAuction.Sql.Context;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.ManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly CarAuctionSqlDbContext _context;

    public VehiclesController(CarAuctionSqlDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetVehicles()
    {
        return Ok(_context.BaseVehicleEntity.ToList());
    }

    [HttpPost("hatchback")]
    public IActionResult AddHatchback([FromBody] Hatchback hatchback)
    {
        _context.Hatchback.Add(hatchback);
        _context.SaveChanges();
        return Ok(hatchback);
    }

    [HttpPost("sudan")]
    public IActionResult AddSedan([FromBody] Sudan sedan)
    {
        _context.Sudan.Add(sedan);
        _context.SaveChanges();
        return Ok(sedan);
    }

    [HttpPost("suv")]
    public IActionResult AddSUV([FromBody] Suv suv)
    {
        _context.Suv.Add(suv);
        _context.SaveChanges();
        return Ok(suv);
    }

    [HttpPost("truck")]
    public IActionResult AddTruck([FromBody] Truck truck)
    {
        _context.Truck.Add(truck);
        _context.SaveChanges();
        return Ok(truck);
    }
}