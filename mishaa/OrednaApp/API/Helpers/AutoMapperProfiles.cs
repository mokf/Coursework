using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>();
        CreateMap<Photo, PhotoDto>();
        CreateMap<Apartment, ApartmentDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt
                    .MapFrom(src => src.Photos
                        .FirstOrDefault(x => x.IsMain).Url));
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<ApartmentUpdateDto, Apartment>();
        CreateMap<Apartment, Apartment>()
            .ForMember(dest => dest.PhotoUrl, 
                opt => opt
                    .MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
        CreateMap<RegisterDto, AppUser>();
        CreateMap<CreateApartmentDto, Apartment>();
    }
}