using CarAuction.Common.Global.Enum;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using System.Diagnostics.CodeAnalysis;

namespace CarAuction.Common.MappingProfiles;

[ExcludeFromCodeCoverage]
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<CreateHatchbackRequest, VehicleDto>()
           .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(_ => VehicleType.Hatchback))
           .ForMember(dest => dest.NumberOfDoors, opt => opt.MapFrom(src => src.NumberOfDoors));

        CreateMap<CreateSedanRequest, VehicleDto>()
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(_ => VehicleType.Sedan))
            .ForMember(dest => dest.NumberOfDoors, opt => opt.MapFrom(src => src.NumberOfDoors));

        CreateMap<CreateSuvRequest, VehicleDto>()
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(_ => VehicleType.Suv))
            .ForMember(dest => dest.NumberOfSeats, opt => opt.MapFrom(src => src.NumberOfSeats));

        CreateMap<CreateTruckRequest, VehicleDto>()
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(_ => VehicleType.Truck))
            .ForMember(dest => dest.LoadCapacity, opt => opt.MapFrom(src => src.LoadCapacity));

        CreateMap<CreateAuctionRequest, Auction>()
            .ForMember(dest => dest.Vehicles, opt => opt.Ignore());


        CreateMap<Vehicle, VehicleDto>().ReverseMap();

        CreateMap<CreateAuctionRequest, Auction>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Auction, AuctionDto>().ReverseMap();

        CreateMap<CreateBidRequest, Bid>().ReverseMap();
        CreateMap<Bid, BidDto>().ReverseMap();

        CreateMap<User, UserDto>().ReverseMap();

    }
}
