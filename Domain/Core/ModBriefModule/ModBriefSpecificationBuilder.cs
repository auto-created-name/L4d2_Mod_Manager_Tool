using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.ModBriefModule
{
    /// <summary>
    /// 模组概要规格构建器
    /// </summary>
    public class ModBriefSpecificationBuilder
    {
        private string filterName = "";
        public List<string> Tags { get; private set; } = new();
        public List<string> Categories { get; private set; } = new();

        public void SetName(string name)
        {
            filterName = name ?? "";
        }

        public void SetTags(IEnumerable<string> tags)
            => Tags = tags.ToList();

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
                Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public void SetCategories(IEnumerable<string> cats)
            => Categories = cats.ToList();

        public void AddCategory(string cat)
            => Categories.Add(cat);

        public void RemoveCategory(string cat)
            => Categories.Remove(cat);

        public ModBriefSpecification FinalSpec
        {
            get => CreateBlurSpec()
                .And(CreateTagsSpec())
                .And(CreateCategoriesSpec());
        }

        private ModBriefSpecification CreateBlurSpec()
        {
            return string.IsNullOrEmpty(filterName)
                ? new MS_Empty()
                : new MS_Blur(filterName);
        }

        private ModBriefSpecification CreateTagsSpec()
        {
            return Tags.Count switch
            {
                0 => new MS_Empty(),
                _ => Tags.Select(x => (ModBriefSpecification)new MS_Tag(x))
                        .Aggregate((x, y) => x.And(y))
            };
        }

        private ModBriefSpecification CreateCategoriesSpec()
        {
            return Categories.Count switch
            {
                0 => new MS_Empty(),
                _ => Categories.Select(x => (ModBriefSpecification)new MS_Category(x))
                    .Aggregate((x, y) => x.And(y))
            };
        }
    }
}
