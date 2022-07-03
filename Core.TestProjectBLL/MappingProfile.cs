using AutoMapper;
using Core.TestProjectModels.Entities;
using Core.TestProjectModels.Models;


namespace Core.DepositBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CUSTOMER_PROFILE, CUSTOMER_PROFILE_MAP>().ReverseMap();
            CreateMap<CUSTOMER_INTRODUCER, CUSTOMER_INTRODUCER_MAP>().ReverseMap();
            CreateMap<CUSTOMER_ADDRESS, CUSTOMER_ADDRESS_MAP>().ReverseMap();
        }
    }
}