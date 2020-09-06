using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface IRulesRepository
    {       
       Task CreatePolicy(List<RuleModel> policyRules);
       Task<bool> CheckProviderLoanExist(Guid LoanProviderId);       
    }
}
