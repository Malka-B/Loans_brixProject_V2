using Loans.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface IManagerLoansRepository
    {
        Task<bool> CheckLoanExist(Guid loanId);
        Task<List<LoanFailureRulesModel>> GetFailureRulesByLoanId(Guid loanId);
        Task UpdateApproveFailureRules(Guid loanId, List<LoanFailureRulesModel> loanFailureRules);
        Task<LoanModel> GetLoanById(Guid loanId);
    }
}
