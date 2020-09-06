using AutoMapper;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Loans.WebApi.DTO;
using Messages.Commands;
using Messages.MessagesOjects;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Loans.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoansService _loansService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public LoansController(ILoansService loansService, IMapper mapper, IMessageSession messageSession)
        {
            _loansService = loansService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost]
        public async Task<Guid> Create([FromBody] LoanDTO loan)
        {
            LoanModel loanModel = _mapper.Map<LoanModel>(loan);
            Guid loanId = await _loansService.Create(loanModel);
            CheckLoanValid loanToCheck = new CheckLoanValid();
            LoanDetails loanDetails = _mapper.Map<LoanDetails>(loanModel);
            loanDetails.LoanId = loanId;
            loanToCheck.LoanDetails = loanDetails;
            await _messageSession.Send(loanToCheck);                
            return loanId;
            //Ok($"Your loan created successfully. Loan Id:{loanId}");           
        }

        [HttpPut("{LoanId}")]
        public async Task Update(Guid loanId, [FromBody] LoanDTO loanToUpdate)
        {
            LoanModel loanModel = _mapper.Map<LoanModel>(loanToUpdate);
            await _loansService.Update(loanId, loanModel);
            CheckLoanValid loanToCheck = new CheckLoanValid();
            LoanDetails loanDetails = _mapper.Map<LoanDetails>(loanModel);
            loanDetails.LoanId = loanId;
            loanToCheck.LoanDetails = loanDetails;            
            await _messageSession.Send(loanToCheck);               
        }
    }
}
