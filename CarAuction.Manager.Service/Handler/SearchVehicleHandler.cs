using AutoMapper;
using CarAuction.Model;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Manager.Service.Handler
{
    public class SearchVehicleHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {

        //public async Task<IEnumerable<VehicleDto>> SearchAsync(
        //    IEnumerable<string> types, string? manufacturer, string? model, int? year)
        //{
        //    var results = new List<VehicleDto>();

        //    foreach (var typeKey in types)
        //    {
        //        if (_delegates.TryGetValue(typeKey, out var func))
        //        {
        //            results.AddRange(await func(manufacturer, model, year));
        //        }
        //    }

        //    return results;
        //}

        //private async Task<IEnumerable<VehicleDto>> FilterVehicles<T>(
        //    IRepository<T, int> repo, string? manufacturer, string? model, int? year)
        //    where T : Vehicle, new()
        //{
        //    var query = repo.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(manufacturer))
        //        query = query.Where(v => v.Manufacturer.Contains(manufacturer));

        //    if (!string.IsNullOrWhiteSpace(model))
        //        query = query.Where(v => v.Model.Contains(model));

        //    if (year.HasValue)
        //        query = query.Where(v => v.Year == year.Value);

        //    var list = await query.ToListAsync();
        //    return mapper.Map<IEnumerable<VehicleDto>>(list);
        //}
    }
}
