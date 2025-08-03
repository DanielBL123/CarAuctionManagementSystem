using CarAuction.Common.Global.Enum;

namespace CarAuction.Dto.Request;

public abstract record CreateVehicleRequest
(
    int Year,
    int StartingBid,
    string Manufacturer,
    string Model,
    string IdentificationNumber,
    VehicleType VehicleType
);

