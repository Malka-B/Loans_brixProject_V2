using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loans.WebApi.DTO
{
    public class ApproveLoanDTO
    {        
        [Required]
        public List<ApproveRuleDTO> approveRules { get; set; }
    }
}
