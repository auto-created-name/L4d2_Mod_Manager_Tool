using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain
{
    public record Mod(
        int Id,
        int FileId,
        string vpkId,
        string Thumbnail, 
        string Title, 
        string Version, 
        string Tagline, 
        string Author,
        string Description,
        ImmutableArray<string> Categories,
        string WorkshopTitle,
        string WorkshopDescript,
        string WorkshopPreviewImage,
        ImmutableArray<string> Tags
        );

    public static class ModFP
    {
        public static Mod CreateMod(ModFile f)
        {
            var vpkNum = GetVpkNumberFromFileName(f.FilePath);
            return new Mod(-1, f.Id, vpkNum.ValueOr(null), 
                null, null, null, null, null, null, ImmutableArray<string>.Empty, 
                null, null, null, ImmutableArray<string>.Empty);
        }

        public static bool HaveVpkNumber(Mod mod) => !string.IsNullOrEmpty(mod.vpkId);

        public static string CategoriesSingleLine(this Mod mod) =>
            string.Join(',', mod.Categories);

        public static string TagsSingleLine(this Mod mod) =>
            string.Join(',', mod.Tags);

        /// <summary>
        /// 从VPK名称上获取VPK号
        /// </summary>
        public static Maybe<string> GetVpkNumberFromFileName(string file)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(file);
            return Service.Spider.IsVpkNumber(name) ?
                Maybe.Some(name) : Maybe.None;
        }

        /// <summary>
        /// 模组信息没有创意工坊信息
        /// </summary>
        public static bool HaveWorkshopInfo(Mod mod)
        {
            return
                !string.IsNullOrEmpty(mod.WorkshopTitle)
                || !string.IsNullOrEmpty(mod.WorkshopDescript)
                || !string.IsNullOrEmpty(mod.WorkshopPreviewImage);
        }

        public static string SelectName(Mod mod)
        {
            if (!string.IsNullOrEmpty(mod.WorkshopTitle))
                return mod.WorkshopTitle;
            else if (!string.IsNullOrEmpty(mod.Title))
                return mod.Title;
            else
                return "<无名称>";
        }

        public static string SelectDescription(Mod mod)
        {
            if (!string.IsNullOrEmpty(mod.WorkshopDescript))
                return mod.WorkshopDescript;
            else
                return mod.Description;
        }

        public static string SelectPreview(Mod mod)
        {
            if (!string.IsNullOrEmpty(mod.WorkshopPreviewImage))
                return mod.WorkshopPreviewImage;
            else
                return mod.Thumbnail;
        }

        /// <summary>
        /// 谓词构造 - 包含指定标签
        /// </summary>
        public static Func<Mod, bool> HaveTag(string tagName)
        {
            return m => m.Tags.Contains(tagName.ToLower());
        }

        /// <summary>
        /// 谓词构造 - 包含分类
        /// </summary>
        public static Func<Mod, bool> HaveCategory(string catName)
        {
            return m => m.Categories.Contains(catName);
        }

        public static Func<Mod, bool> NameContains(string name)
        {
            return m =>
                m.Title.Contains(name, StringComparison.CurrentCultureIgnoreCase)
                || m.WorkshopTitle.Contains(name, StringComparison.CurrentCultureIgnoreCase);
        }

        public static Func<Mod, bool> AuthorContains(string name)
        {
            return m => m.Author.Contains(name, StringComparison.CurrentCultureIgnoreCase);
        }

        public static Func<Mod, bool> VPKIdContains(string name)
        {
            return m => m.vpkId.Contains(name, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public class ModEqualityComparer : IEqualityComparer<Mod>
    {
        public bool Equals(Mod x, Mod y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Mod obj)
        {
            return obj.Id;
        }
    }
}
