using AutoMapper;
using Domain.Entities;
using Web_Api.Contracts;

namespace Web_Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Property, PropertyContract>();
        CreateMap<AddPropertyContract, Property>();
        CreateMap<UpdatePropertyContract, Property>();

        CreateMap<RoomType, RoomTypeContract>();
        CreateMap<AddRoomTypeContract, RoomType>();
        CreateMap<UpdateRoomTypeContract, RoomType>();

        CreateMap<Reservation, ReservationContract>();
        CreateMap<AddReservationContract, Reservation>();
    }
}