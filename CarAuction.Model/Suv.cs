namespace CarAuction.Model;
public record SUV : Vehicle
{
    public int NumberOfSeats { get; init; }

    public SUV(string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
        : base(manufacturer, model, year, startingBid)
    {
        NumberOfSeats = numberOfSeats;
    }

}