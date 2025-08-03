namespace CarAuction.Common.MappingProfiles.Extensions;

public static class MappingExtensions
{
    public static TDest MapTo<TDest>(this object src, IMapper mapper) =>
        (TDest)mapper.Map(src, src.GetType(), typeof(TDest));

    public static IEnumerable<TDest> MapTo<TDest>(this IQueryable<object> src, IMapper mapper) =>
        (IEnumerable<TDest>)mapper.Map(src, src.GetType(), typeof(IEnumerable<TDest>));

    public static IEnumerable<TDest> MapTo<TDest>(this IEnumerable<object> src, IMapper mapper) =>
        (IEnumerable<TDest>)mapper.Map(src, src.GetType(), typeof(IEnumerable<TDest>));
}
