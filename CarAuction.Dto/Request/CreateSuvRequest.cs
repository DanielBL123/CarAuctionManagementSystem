using CarAuction.Common.Global.Enum;

namespace CarAuction.Dto.Request;

public record CreateSuvRequest(
    int Year,
    int StartingBid, 
    string Manufacturer,
    string Model,
    string IdentificationNumber,
    int NumberOfSeats
) : CreateVehicleRequest(Year, StartingBid, Manufacturer, Model, IdentificationNumber, VehicleType.Suv);