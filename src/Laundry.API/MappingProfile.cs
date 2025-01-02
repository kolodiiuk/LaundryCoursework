using AutoMapper;
using Laundry.API.Dto;
using Laundry.Domain.Entities;

namespace Laundry.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Service, CreateServiceDto>().ReverseMap();
        CreateMap<Service, UpdateServiceDto>().ReverseMap();
        // CreateMap<Service, UpdateServiceDto>();
    }
}