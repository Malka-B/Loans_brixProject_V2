using AutoMapper;
using Loans.Data.Entities;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.Data
{
    public class ManagerLoansRepository : IManagerLoansRepository
    {
        private readonly LoansContext _loansContext;
        private readonly IMapper _mapper;

        public ManagerLoansRepository(LoansContext loansContext, IMapper mapper)
        {
            _loansContext = loansContext;
            _mapper = mapper;
        }

        public async Task<bool> CheckLoanExist(Guid loanId)
        {
            LoanEntity loanEntity = await _loansContext.Loans
                .FirstOrDefaultAsync(l => l.ID == loanId);
            if (loanEntity == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<LoanFailureRulesModel>> GetFailureRulesByLoanId(Guid loanId)
        {
            LoanEntity loanEntity = await _loansContext.Loans
                .Include(l => l.FailureRules)
                .FirstOrDefaultAsync(l => l.ID == loanId);
            List<LoanFailureRulesModel> loanFailureRules = _mapper.Map<List<LoanFailureRulesModel>>(loanEntity.FailureRules);
            return loanFailureRules;
        }

        public async Task<LoanModel> GetLoanById(Guid loanId)
        {
            LoanEntity loanEntity = await _loansContext.Loans
                .FirstOrDefaultAsync(l => l.ID == loanId);
            LoanModel loan = _mapper.Map<LoanModel>(loanEntity);
            return loan;
        }

        public async Task UpdateApproveFailureRules(Guid loanId, List<LoanFailureRulesModel> loanFailureRules)
        {
            List<LoanFailureRulesEntity> approveFailureRules = _mapper.Map<List<LoanFailureRulesEntity>>(loanFailureRules);
            LoanEntity loanEntity = await _loansContext.Loans                
                .FirstOrDefaultAsync(l => l.ID == loanId);
            loanEntity.FailureRules = approveFailureRules;
            await _loansContext.SaveChangesAsync();
        }
    }
}
