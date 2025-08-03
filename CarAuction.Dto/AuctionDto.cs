namespace CarAuction.Dto;

public class AuctionDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IList<VehicleDto> Vehicles { get; set; } = null!;
}
     

