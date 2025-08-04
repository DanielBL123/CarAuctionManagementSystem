namespace CarAuction.Dto;

public class BidDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int VehicleId { get; set; }
    public int UserId { get; set; }
    public int Amount { get; set; }
    public DateTime Timestamp { get; set; }
}

