using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.ModFilter
{
    /// <summary>
    /// 模组过滤器构建器
    /// </summary>
    class ModFilterBuilder
    {
        private string filterName = "";
        public List<string> Tags { get; private set; } = new();
        public IModFilter FinalFilter
        {
            get => CreateNameFilter().And(CreateTagsFilter());
        }

        public void SetName(string name)
        {
            filterName = name ?? "";
        }

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
                Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        private IModFilter CreateNameFilter() 
        {
            if (string.IsNullOrEmpty(filterName))
                return new EmptyFilter();
            else
                return new ModBlurFilter(filterName);
        }

        private IModFilter CreateTagsFilter()
        {
            if (Tags.Count == 0)
            {
                return new EmptyFilter();
            }
            else
            {
                return
                    Tags.Select(ModFP.HaveTag)
                    .Select(x => (IModFilter)new PredicateModFilter(x))
                    .Aggregate((x1, x2) => new AndFilter(x1, x2));
            }
        }
    }
}
