using AutoMapper;
using Rules.Data.Entities;
using Rules.Service.Models;

namespace Rules.Data.Profiles
{
    public class RulesRepositoryProfile : Profile
    {
        public RulesRepositoryProfile()
        {
            CreateMap<Rule, RuleModel>();
            CreateMap<RulesParameter, RulesParameterModel>();
            CreateMap<RuleModel, Rule>()               
                .ForMember(destination => destination.Id , option => option.Ignore()); 
        }
    }
}
