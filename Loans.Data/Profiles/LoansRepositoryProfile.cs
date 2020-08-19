using AutoMapper;
using Loans.Data.Entities;
using Loans.Service.Models;

namespace Loans.Data.Profiles
{
    public class LoansRepositoryProfile : Profile
    {
        public LoansRepositoryProfile()
        {
            CreateMap<LoanModel, LoanEntity>();
            CreateMap<LoanEntity, LoanModel>();
            CreateMap<LoanFailureRulesModel, LoanFailureRulesEntity>();
            CreateMap<LoanFailureRulesEntity, LoanFailureRulesModel>();            
        }
    }
}
