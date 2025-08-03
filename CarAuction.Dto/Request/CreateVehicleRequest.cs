using CarAuction.Common.Global.Enum;
using System.Text.Json.Serialization;

namespace CarAuction.Dto.Request;

public abstract record CreateVehicleRequest
(
    int Year,
    int StartingBid,
    string Manufacturer,
    string Model,
    string IdentificationNumber,
    [property: JsonIgnore] VehicleType VehicleType
);

