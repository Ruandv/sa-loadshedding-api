namespace EskomCalendarApi.Mappings
{
    using AutoMapper;
    using global::Models.Eskom;
    using global::Models.Eskom.ResponseDto;

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Suburb, SuburbSearchResponseDto>();
            CreateMap<SuburbSearch, SuburbSearchResponseDto>().ForMember(dest => dest.BlockId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SuburbData, SuburbSearchResponseDto>()
                .ForMember(dest => dest.BlockId, opt => opt.MapFrom(src => src.BlockId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SubName));
        }
    }
}
