namespace CarAuction.Model;

public record Truck : Vehicle
{
    public double LoadCapacity { get; init; }

    public Truck(string manufacturer, string model, int year, decimal startingBid, double loadCapacity)
        : base(manufacturer, model, year, startingBid)
    {
        LoadCapacity = loadCapacity;
    }
}

