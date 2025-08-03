namespace CarAuction.Dto.Request;

public record CreateBidRequest(
    string AuctionName,
    string VehicleUniqueIdentifier,
    int Amount
);
