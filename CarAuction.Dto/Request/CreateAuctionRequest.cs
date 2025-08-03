namespace CarAuction.Dto.Request;

public record CreateAuctionRequest
(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    IEnumerable<string> VehicleIdentificationNumbers
);
