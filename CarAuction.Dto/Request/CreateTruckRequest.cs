using CarAuction.Common.Global.Enum;

namespace CarAuction.Dto.Request;

public record CreateTruckRequest(
    int Year,
    int StartingBid,
    string Manufacturer,
    string Model,
    string IdentificationNumber,
    double LoadCapacity
) : CreateVehicleRequest(Year, StartingBid, Manufacturer, Model, IdentificationNumber, VehicleType.Truck);
