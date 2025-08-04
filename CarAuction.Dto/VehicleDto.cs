using CarAuction.Common.Global.Enum;
using System.Text.Json.Serialization;

namespace CarAuction.Dto;

public class VehicleDto()
{
    [property: JsonIgnore]
    public int Id { get; set; }
    public int Year { get; set; }
    public int StartingBid { get; set; }
    public int? AuctionId { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }
    public int? CurrentBidAmount { get; set; }
    public double? LoadCapacity { get; set; }
    public bool IsSold { get; set; }
    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string IdentificationNumber { get; set; } = null!;
    public VehicleType VehicleType { get; set; }
    public VehicleAction VehicleAction { get; set; }

}