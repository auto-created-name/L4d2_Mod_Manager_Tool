using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;

namespace Domain.ModLocalInfo
{
    public class ModAnalysisServer
    {
        private CategoryRuleGroup ruleGroup;

        public ModAnalysisServer()
        {
            // 初始化内容分类规则
            ruleGroup = new CategoryRuleGroupFactory().Create();
        }

        public LocalInfo AnalysisMod(ModFile.ModFile mf)
        {
            var fn = Path.Combine(L4d2Folder.AddonsFolder, mf.FileLoc);
            if (!File.Exists(fn)) return null;

            var vpkFile = new VpkFile(fn);

            LocalInfo li = new();
            li.Categories = vpkFile.Categories;
            li.AddonImage = vpkFile.Image.ValueOr(ImageFile.MissingImage);
            vpkFile.Info.Match(ai => 
            {
                li.Title = ai.Title;
                li.Author = ai.Author;
                li.Version = ai.Version;
                li.Tagline = ai.Tagline;
                li.Description = ai.Description;
                li.Categories = li.Categories.Concat(ai.Categories).Distinct().ToArray();
            }, () => { });
            return li;
        }
    }
}
