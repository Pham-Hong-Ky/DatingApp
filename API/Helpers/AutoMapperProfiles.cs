

using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUsers, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                    src.Photos.FirstOrDefault(x => 
                        x.IsMain).url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CaculateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}