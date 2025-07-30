namespace CarAuction.Model;

public record Hatchback : Vehicle
{
    public int NumberOfDoors { get; init; }

    public Hatchback(string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
        : base(manufacturer, model, year, startingBid)
    {
        NumberOfDoors = numberOfDoors;
    }
}
