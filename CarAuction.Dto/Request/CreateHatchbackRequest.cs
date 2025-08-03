using CarAuction.Common.Global.Enum;

namespace CarAuction.Dto.Request;

public record CreateHatchbackRequest(
    int Year,
    int StartingBid,
    string Manufacturer,
    string Model,
    string IdentificationNumber,
    int NumberOfDoors
) : CreateVehicleRequest(Year, StartingBid, Manufacturer, Model, IdentificationNumber, VehicleType.Hatchback);

