using CarAuction.Common.Global.Enum;
using CarAuction.Model.BaseEntities;

namespace CarAuction.Model;

public class Auction : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public AuctionStatus Status { get; set; } = AuctionStatus.Active;
    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Bid> Bids { get; set; } = [];
}
