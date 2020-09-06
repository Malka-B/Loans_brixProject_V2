using Rules.Service.Models;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface IRulesService
    {             
        Task RegisterToProvideLoans(RegisterModel registerModel);
    }
}
