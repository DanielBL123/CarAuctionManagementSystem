using CarAuction.Common.Global.Enum;
using CarAuction.Model.BaseEntities;

namespace CarAuction.Model;

public class Vehicle : IEntity<int>
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int StartingBid { get; set; }
    public int? AuctionId { get; set; }
    public int? UserId { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }
    public double? LoadCapacity { get; set; }
    public bool IsSold { get; set; }
    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string IdentificationNumber { get; set; } = null!;
    public VehicleType VehicleType { get; set; }
    public VehicleAction VehicleAction { get; set; } = VehicleAction.None;
    public Auction? Auction { get; set; } = null!;
    public User? User { get; set; } = null!;
    public ICollection<Bid> Bids { get; set; } = [];

}
