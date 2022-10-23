using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    record CategoryRule(string Category, string Regular, string Path);
    internal class CategoryRuleGroup
    {
        private List<CategoryRule> categoryRules = new();
        public CategoryRuleGroup(List<CategoryRule> rules)
        {
            categoryRules = rules;
        }

        public string[] Pathes => categoryRules.Select(x => x.Path).ToArray();

        public string[] MatchCategories(string entry)
        {
            return categoryRules
                .Where(r => Regex.IsMatch(entry, r.Regular))
                .Select(r => r.Category).ToArray();
        }
    }
}
