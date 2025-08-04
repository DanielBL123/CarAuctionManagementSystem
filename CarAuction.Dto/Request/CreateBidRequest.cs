namespace CarAuction.Dto.Request;

public record CreateBidRequest(
    string AuctionName,
    int Amount
);
