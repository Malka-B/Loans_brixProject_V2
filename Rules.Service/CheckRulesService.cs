using AutoMapper;
using Exceptions;
using Messages.Commands;
using Messages.Events;
using Messages.MessagesOjects;
using Rules.Service.Extensions;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rules.Service
{
    public class CheckRulesService : ICheckRulesService
    {
        private readonly ICheckRulesRepository _checkRulesRepository;
        private readonly IMapper _mapper;        
        private readonly Dictionary<string, string> rulesParameterTypes = new Dictionary<string, string>() {
            { "Salary", "int" },
            { "FixedSalary", "bool"},
            {"Age", "int" },
            {"LoanAmount", "int" },
            {"SeniorityYearsInBank", "int" },
            {"NumberOfRepayments", "int" },
            {"LoanPurpose", "string" }
        };

        public CheckRulesService(ICheckRulesRepository checkRulesRepository, IMapper mapper)
        {
            _checkRulesRepository = checkRulesRepository;
            _mapper = mapper;          
        }

        public async Task<LoanChecked> CheckLegality(CheckLoanValid loanToCheck)
        {
            List<RuleModel> rulesTree = await _checkRulesRepository.getRules(loanToCheck.LoanDetails.LoanProviderId);
            if (rulesTree.Count() == 0)
            {
                throw new LoanProviderNotFoundException();
                //write to db log
            }
            LoanChecked loanChecked = ScanProviderTreeRules(rulesTree, loanToCheck);
            loanChecked.LoanId = loanToCheck.LoanDetails.LoanId;
            return loanChecked;
        }

        private LoanChecked ScanProviderTreeRules(List<RuleModel> rulesTree, CheckLoanValid loanToCheck)
        {
            LoanChecked loanChecked = new LoanChecked();
            bool isBranchRulesValid = true;
            // checkedRulesTree that contains what scanned in oreder to represent it to client
            List<RuleTreeNode> checkedRulesTree = _mapper.Map<List<RuleTreeNode>>(rulesTree);
            List<RuleModel> ruleRoots = rulesTree.Where(rl => rl.ParentRule == null).ToList();
            List<RuleTreeNode> CheckedRulesRoots = checkedRulesTree.Where(rl => rl.ParentRuleId == 0).ToList();
            for (int i = 0; i < ruleRoots.Count; i++)
            {
                isBranchRulesValid = CheckBranchRulesValid(ruleRoots[i], CheckedRulesRoots[i], loanToCheck);
                if (isBranchRulesValid)
                {
                    loanChecked = CreateValidCheckedLoan(checkedRulesTree);
                    return loanChecked;
                }
            }
            loanChecked = CreateInvalidCheckedLoan(checkedRulesTree);
            return loanChecked;
        }

        private LoanChecked CreateInvalidCheckedLoan(List<RuleTreeNode> checkedRulesTree)
        {
            LoanChecked checkedLoan = new LoanChecked
            {
                IsLoanValid = false,
                CheckedRuleTree = checkedRulesTree
            };

            return checkedLoan;
        }

        private LoanChecked CreateValidCheckedLoan(List<RuleTreeNode> checkedRulesTree)
        {
            LoanChecked checkedLoan = new LoanChecked
            {
                IsLoanValid = true,
                CheckedRuleTree = checkedRulesTree
            };
            return checkedLoan;
        }

        private bool CheckBranchRulesValid(RuleModel ruleRoot, RuleTreeNode checkedRulesRoot, CheckLoanValid message)
        {
            //depth tree scan using stack
            bool isBranchValid = true;
            Stack<RuleModel> rulesTreeStack = new Stack<RuleModel>();
            Stack<RuleTreeNode> checkedRulesTreeStack = new Stack<RuleTreeNode>();

            rulesTreeStack.Push(ruleRoot);
            checkedRulesTreeStack.Push(checkedRulesRoot);
            while (!(rulesTreeStack.Count == 0))
            {
                var currentRuleToCheck = rulesTreeStack.Pop();
                var currentCheckedRuleToUpdate = checkedRulesTreeStack.Pop();

                for (int j = 0; j < currentRuleToCheck.ChildrenRules.Count; j++)//create stack of branch nodes
                {
                    rulesTreeStack.Push(currentRuleToCheck.ChildrenRules[j]);
                    checkedRulesTreeStack.Push(currentCheckedRuleToUpdate.ChildrenRules[j]);
                }
                var loanValueToCompare = message.LoanDetails.GetType().GetProperty(currentRuleToCheck.Parameter).GetValue(message.LoanDetails, null);
                bool isRuleValid = CheckRuleValid(loanValueToCompare, currentRuleToCheck.Parameter, currentRuleToCheck.Operator, currentRuleToCheck.ValueToCompeare, currentRuleToCheck.Id, message.IgnoreRules);
                isBranchValid = isRuleValid ? isBranchValid : false;
                UpdateCheckedRulesTree(loanValueToCompare, currentCheckedRuleToUpdate, currentRuleToCheck, isRuleValid);

                if (currentRuleToCheck.ChildrenRules.Count == 0)//at end of branch
                {
                    if (isBranchValid)
                    {
                        return true;
                    }                    
                }
            }
            return false;
        }

        private void UpdateCheckedRulesTree(object loanValueToCompare, RuleTreeNode ruleTreeNodeStackItem, RuleModel ruleTreeStackItem, bool isRuleValid)
        {
            ruleTreeNodeStackItem.IsRuleValid = isRuleValid;
            ruleTreeNodeStackItem.RuleDescription = isRuleValid ? GetValidRuleDescription(loanValueToCompare, ruleTreeStackItem) :
                GetInvalidRuleDescription(loanValueToCompare, ruleTreeStackItem);
        }

        private string GetInvalidRuleDescription(object loanValueToCompare, RuleModel ruleTreeStackItem)
        {
            string ruleDescription =
                           $"Invalid: {ruleTreeStackItem.Parameter} = {loanValueToCompare} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
            return ruleDescription;
        }
        //send 3 params 1,1.....
        private bool CheckRuleValid(object loanValueToCompare, string parameter, string @operator, string valueToCompare, int id, List<int> ignoreRules)
        {
            if (ignoreRules != null && ignoreRules.Contains(id))//approve by manager
            {
                return true;
            }
            var parameterType = rulesParameterTypes[parameter];
            switch (parameterType)
            {
                case "int":
                    Expression<Func<int, bool>> intRuleExpression = GetRuleExpression(parameter, @operator, Convert.ToInt32(valueToCompare));
                    return intRuleExpression.Compile()((int)loanValueToCompare);
                case "bool":
                    Expression<Func<bool, bool>> boolRuleExpression = GetRuleExpression(parameter, @operator, Convert.ToBoolean(valueToCompare));
                    return boolRuleExpression.Compile()((bool)loanValueToCompare);
                case "string":
                    Expression<Func<string, bool>> stringRuleExpression = GetRuleExpression(parameter, @operator, Convert.ToString(valueToCompare));
                    return stringRuleExpression.Compile()((string)loanValueToCompare);
                default:
                    return false;
            }
        }

        private string GetValidRuleDescription(object loanValue, RuleModel ruleTreeStackItem)
        {
            string ruleDescription =
               $"Valid rule: {ruleTreeStackItem.Parameter} = {loanValue} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
            return ruleDescription;
        }

        private Expression<Func<T, bool>> GetRuleExpression<T>(string parameter, string operator1, T valToCompare)

        {
            ParameterExpression param = Expression.Parameter(typeof(T), parameter);//שם פרמטר הלמדה וסוג התכולה שלו            
            ConstantExpression valueToCompare = Expression.Constant(valToCompare);//הערך אליו משווים והסוג שלו 

            BinaryExpression parameterOperatorValToCompare = operator1.Operator(param, valueToCompare);// LessThan אופרטור ההשוואה
            Expression<Func<T, bool>> ruleExpression =
                Expression.Lambda<Func<T, bool>>(
                    parameterOperatorValToCompare,
                    new ParameterExpression[] { param });
            return ruleExpression;
        }
    }
}


