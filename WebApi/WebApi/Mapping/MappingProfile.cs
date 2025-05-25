using AutoMapper;
using Domain.Entities;
using WebApi.Contracts;

namespace WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Property, PropertyResponseContract>();
        CreateMap<AddPropertyContract, Property>();
        CreateMap<UpdatePropertyContract, Property>();
        CreateMap<Property, PropertySearchResult>();

        CreateMap<RoomType, RoomTypeResponseContract>();
        CreateMap<AddRoomTypeContract, RoomType>();
        CreateMap<UpdateRoomTypeContract, RoomType>();
        CreateMap<RoomType, RoomTypeSearchResult>();

        CreateMap<Reservation, ReservationResponse>();
        CreateMap<AddReservationRequestContract, Reservation>();
    }
}