using AutoMapper;
using Rules.Service.Models;
using Rules.WebApi.DTO;

namespace Rules.WebApi.Profiles
{
    public class RulesApiProfile : Profile
    {
        public RulesApiProfile()
        {
            CreateMap<RegisterDTO, RegisterModel>();
        }
    }
}
