using Messages.Commands;
using Messages.Events;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface ICheckRulesService
    {
        Task<LoanChecked> CheckLegality(CheckLoanValid message);
    }
}
