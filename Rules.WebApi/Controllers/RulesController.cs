using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using Rules.WebApi.DTO;
using System;
using System.Threading.Tasks;

namespace Rules.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesService _rulesService;
        private readonly IMapper _mapper;

        public RulesController(IRulesService rulesService, IMapper mapper)
        {
            _rulesService = rulesService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task RegisterToProvideLoans(RegisterDTO register)
        {
            RegisterModel registerModel = _mapper.Map<RegisterModel>(register);
            await _rulesService.RegisterToProvideLoans(registerModel); 
        }        
    }
}
