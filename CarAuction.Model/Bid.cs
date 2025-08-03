using CarAuction.Model.BaseEntities;

namespace CarAuction.Model;

public class Bid : IEntity<int>
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int VehicleId { get; set; }
    public int UserId { get; set; }
    public int Amount { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Auction Auction { get; set; } = null!;
    public User User { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
}