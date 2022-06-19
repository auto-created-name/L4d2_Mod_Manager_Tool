using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Domain
{
    public record Mod(
        int Id,
        int FileId,
        string Thumbnail, 
        string Title, 
        string Version, 
        string Tagline, 
        string Author,
        string WorkshopTitle,
        string WorkshopDescript,
        string WorkshopPreviewImage
        );

    public static class ModFP
    {
        public static Mod CreateMod(ModFile f)
        {
            return new Mod(-1, f.Id, null, null, null, null, null, null, null, null);
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
    }
}
