using Rules.Service.Models;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface IRulesService
    {                
        Task UpdatePolicyRules(RegisterModel registerModel);
        Task RegisterToProvideLoans(RegisterModel registerModel);
    }
}
