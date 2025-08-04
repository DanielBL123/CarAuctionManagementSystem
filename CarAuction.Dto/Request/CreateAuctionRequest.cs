namespace CarAuction.Dto.Request;

public record CreateAuctionRequest()
{
    public string Name { get; set; } = null!;
    public DateTime StartDateTime = DateTime.Now;
    public IEnumerable<string> VehicleIdentificationNumbers { get; set; } = null;
}

