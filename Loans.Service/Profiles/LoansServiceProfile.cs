using AutoMapper;
using Loans.Service.Models;
using Messages.Commands;
using Messages.MessagesOjects;
using System;

namespace Loans.Service.Profiles
{
    public class LoansServiceProfile : Profile
    {
        public LoansServiceProfile()
        {
            CreateMap<RuleTreeNode, LoanFailureRulesModel>();
            CreateMap<LoanModel, LoanDetails>()
                .ForMember(destination => destination.Age, option => option.MapFrom(src =>
                (DateTime.Now.Year - src.Birthdate.Year)));
        }      
    }
}
