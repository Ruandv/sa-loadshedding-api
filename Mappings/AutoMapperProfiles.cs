namespace EskomCalendarApi.Mappings
{
    using AutoMapper;
    using global::Models.Eskom;
    using global::Models.Eskom.ResponseDto;

    public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap <SuburbSearch, SuburbSearchResponseDto > ();
        //CreateMap<Order, OrderViewModel>();
        // Add more mapping profiles here as needed
    }
}
}
