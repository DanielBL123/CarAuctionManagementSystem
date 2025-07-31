namespace CarAuction.Model.BaseEntities;
public interface IEntity<T> where T : IComparable, IEquatable<T>
{
    T Id { get; set; }
}
