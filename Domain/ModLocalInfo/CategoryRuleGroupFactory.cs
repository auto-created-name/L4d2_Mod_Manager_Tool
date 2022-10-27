using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    struct Json_CategoryRule
    {
        [JsonProperty("regular")]
        public string Regular;
        [JsonProperty("category")]
        public string Category;
    }

    public class CategoryRuleGroupFactory
    {
        private const string CategoriesConfigureFile = "Resources/categories.json";

        public CategoryRuleGroup Create()
        {
            try
            {
                var jsonModel = JsonConvert.DeserializeObject<List<Json_CategoryRule>>(
                    File.ReadAllText(CategoriesConfigureFile));
                var categoryRules = jsonModel.Select(m =>
                {
                    int lastIndex = m.Category.LastIndexOf('/') + 1;
                    if (lastIndex > 0)
                        return new CategoryRule(m.Category.Substring(lastIndex), m.Regular, m.Category);
                    else
                        return new CategoryRule(m.Category, m.Regular, m.Category);
                }).ToList();
                return new CategoryRuleGroup(categoryRules);
            }
            catch (Exception e)
            {
                throw new Exception("分类规则加载失败:" + e.Message);
            }
        }
    }
}
