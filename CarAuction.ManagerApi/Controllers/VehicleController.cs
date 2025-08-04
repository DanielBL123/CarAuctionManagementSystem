using CarAuction.Common.Global.Enum;
using CarAuction.Dto;


namespace CarAuction.ManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IVehicleService vehicleService) : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] List<VehicleType>? types,
                                                 [FromQuery] string? manufacturer,
                                                 [FromQuery] string? model,
                                                 [FromQuery] int? year)
    {
        var result = await vehicleService.GetVehiclesAsync(types, manufacturer, model, year);
        return Ok(result);
    }

    [HttpPost("hatchback")]
    public async Task<IActionResult> AddHatchback([FromBody] CreateHatchbackRequest hatchback)
        => await HandleAddVehicle(hatchback);

    [HttpPost("sedan")]
    public async Task<IActionResult> AddSedan([FromBody] CreateSedanRequest sedan)
        => await HandleAddVehicle(sedan);

    [HttpPost("suv")]
    public async Task<IActionResult> AddSuv([FromBody] CreateSuvRequest suv)
        => await HandleAddVehicle(suv);

    [HttpPost("truck")]
    public async Task<IActionResult> AddTruck([FromBody] CreateTruckRequest truck)
        => await HandleAddVehicle(truck);

    private async Task<IActionResult> HandleAddVehicle<T>(T request) where T : CreateVehicleRequest
    {
        try
        {
            var result = await vehicleService.AddVehicle(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}