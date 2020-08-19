using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rules.WebApi.DTO
{
    public class RegisterDTO
    {
        public string FilePath { get; set; }
        public Guid ProviderLoanId { get; set; }
    }
}
