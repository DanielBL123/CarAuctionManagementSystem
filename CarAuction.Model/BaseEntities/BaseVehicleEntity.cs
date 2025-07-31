namespace CarAuction.Model.BaseEntities;

public abstract class BaseVehicleEntity : IEntity<int>
{
    public int Id { get; set; }
    public int Year { get; set; }
    public decimal StartingBid { get; set; }
    public string Manufacturer { get; set; } = null!;
    public string Model { get; set; } = null!;

}
