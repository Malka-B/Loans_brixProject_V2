using Loans.Service.Models;
using Messages.Events;
using System;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface ILoansService
    {
        Task<Guid> Create(LoanModel loanModel);
        Task Update(Guid loanId, LoanModel loanModel);
        Task UpdateLoanAfterCheckLegality(LoanChecked message);
    }
}
