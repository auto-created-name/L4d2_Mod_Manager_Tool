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
    struct CategoryRule
    {
        [JsonProperty("regular")]
        public string Regular;
        [JsonProperty("category")]
        public string Category;
    }

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
                categoryRules = JsonConvert.DeserializeObject<List<CategoryRule>>(
                    File.ReadAllText(CategoriesConfigureFile));
            }
            catch {}
        }

        public static IEnumerable<string> MatchCategories(string entry)
        {
            return categoryRules
                .Where(r => Regex.IsMatch(entry, r.Regular))
                .Select(r => r.Category);
        }
    }
}
