using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace L4d2_Mod_Manager_Tool.Service
{
    struct Json_CategoryRule
    {
        [JsonProperty("regular")]
        public string Regular;
        [JsonProperty("category")]
        public string Category;
    }

    record CategoryRule(string Category, string Regular, string Path);

    static class ModCategoryService
    {
        private const string CategoriesConfigureFile = "Resources/categories.json";

        private static List<CategoryRule> categoryRules = new();

        /// <summary>
        /// 从文件载入数据
        /// </summary>
        public static void Load()
        {
            try
            {
                var jsonModel = JsonConvert.DeserializeObject<List<Json_CategoryRule>>(
                    File.ReadAllText(CategoriesConfigureFile));
                categoryRules = jsonModel.Select(m =>
                {
                    int lastIndex = m.Category.LastIndexOf('/') + 1;
                    if(lastIndex > 0) 
                        return new CategoryRule(m.Category.Substring(lastIndex), m.Regular, m.Category);
                    else
                        return new CategoryRule(m.Category, m.Regular, m.Category);
                }).ToList();
            }
            catch (Exception e){
                Utility.WinformUtility.ErrorMessageBox("分类规则加载失败:" + e.Message, "服务初始化错误");
            }
        }

        public static IEnumerable<string> Pathes => categoryRules.Select(x => x.Path);

        public static IEnumerable<string> MatchCategories(string entry)
        {
            return categoryRules
                .Where(r => Regex.IsMatch(entry, r.Regular))
                .Select(r => r.Category);
        }
    }
}
