using CarAuction.Dto;


namespace CarAuction.ManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IVehicleService vehicleService, IMapper mapper) : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] List<string>? types,
                                                 [FromQuery] string? manufacturer,
                                                 [FromQuery] string? model,
                                                 [FromQuery] int? year)
    {
        var result = await vehicleService.SearchVehiclesAsync(types, manufacturer, model, year);
        return Ok(result);
    }

    [HttpPost("hatchback")]
    public async Task<IActionResult> AddHatchback([FromBody] CreateHatchbackRequest hatchback)
    {
        try
        {
            return Ok(await vehicleService.AddVehicle(hatchback));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("sedan")]
    public async Task<IActionResult> AddSedan([FromBody] CreateSedanRequest sedan) =>
        Ok(await vehicleService.AddVehicle(sedan));

    [HttpPost("suv")]
    public async Task<IActionResult> AddSuv([FromBody] CreateSuvRequest suv) =>
        Ok(await vehicleService.AddVehicle(suv));


    [HttpPost("truck")]
    public async Task<IActionResult> AddTruck([FromBody] CreateTruckRequest truck) =>
        Ok(await vehicleService.AddVehicle(truck));



}