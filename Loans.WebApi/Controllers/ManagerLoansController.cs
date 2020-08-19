using AutoMapper;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Loans.WebApi.DTO;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManagerLoansController : ControllerBase
    {
        private readonly IManagerLoansService _managerLoansService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public ManagerLoansController(IManagerLoansService managerLoansService, IMapper mapper, IMessageSession messageSession)
        {
            _managerLoansService = managerLoansService;
            _mapper = mapper;
            _messageSession = messageSession;                        
        }

        [HttpPost("{loanId}")]
        public async Task ApproveLoan(Guid loanId, List<ApproveRuleDTO> approveRules)
        {
            List<ApproveRuleModel> approveRulesModel = _mapper.Map<List<ApproveRuleModel>>(approveRules);
            ApproveLoanModel approveLoanModel = new ApproveLoanModel()
            {
                approveRules = approveRulesModel,
                LoanId = loanId
            };
            CheckLoanValid checkLoanValid = await _managerLoansService.ApproveLoan(approveLoanModel);                       
            await _messageSession.Send(checkLoanValid)
                .ConfigureAwait(false);            
        }
    }
}
