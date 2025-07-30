namespace CarAuction.Model;

public record Sudan : Vehicle
{
    public int NumberOfDoors { get; init; }

    public Sudan(string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
        : base(manufacturer, model, year, startingBid)
    {
        NumberOfDoors = numberOfDoors;
    }
}
