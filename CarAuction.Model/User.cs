using CarAuction.Model.BaseEntities;

namespace CarAuction.Model;
public class User : IEntity<int>
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public ICollection<Bid> Bids { get; set; } = [];
    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
