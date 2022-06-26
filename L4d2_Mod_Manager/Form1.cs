using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L4d2_Mod_Manager.Domain;
using L4d2_Mod_Manager.Domain.Repository;
using L4d2_Mod_Manager.Service;

namespace L4d2_Mod_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateModList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModFileRepository rp = new ModFileRepository();
            var mfs = VPKServices.ScanAllModFile().Select(mf => rp.SaveModFile(mf)).ToArray();

            var res = mfs
                .Select(x => VPKServices.ExtraMod(x))
                .Select(x => ModRepository.Instance.SaveMod(x))
                .ToArray();
            //var snippets = VPKServices.ScanVPK();
            //foreach(var snip in snippets)
            //{
            //    var mod = ModFP.CreateMod(snip.VpkName);
            //
            //    // 赋值缩略图信息
            //    mod = snip.AddonImage.Match(img => mod with { Thumbnail = img }, () => mod);
            //    mod = snip.AddonInfo.Match(info =>
            //    {
            //        var modinfo = ModOperation.ReadModInfo(info);
            //        return mod with
            //        {
            //            Title = modinfo.Title.ValueOr(""),
            //            Version = modinfo.Version.ValueOr(""),
            //            Tagline = modinfo.Tagline.ValueOr(""),
            //            Author = modinfo.Author.ValueOr("")
            //        };
            //    }, () => mod);
            //    ModRepository.Instance.SaveMod(mod);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private void UpdateModList()
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();

            int index = 0;

            foreach (var mod in ModOperation.ModInfos())
            {
                var img = SelectImage(mod.img);
                imageList1.Images.Add(img);

                ListViewItem item = new(new string[] {
                    mod.name,
                    mod.vpkid,
                    mod.path
                });
                //item.Text = ModFP.SelectName(mod.img);
                //item.SubItems.Add(mod.vpkId);
                item.ImageIndex = index++;
                item.Tag = mod.id;
                listView1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var a = ModRepository.Instance.GetMods()
                .Where(x => !ModFP.HaveWorkshopInfo(x))
                .Select(x => ModOperation.UpdateWorkshopInfo(x))
                .Where(x => x.Item2)
                .Select(x => x.Item1)
                .Select(x => ModOperation.MoveWorkshopPreviewImage(x))
                .ToArray();
            foreach(var b in a)
            {
                ModRepository.Instance.UpdateMod(b);
            }
        }

        private static Image SelectImage(string img)
        {
            return LoadImageSafe(img).ValueOr(
                    Image.FromFile(@"C:\Users\Louis\Desktop\no-image.png"));
        }

        /// <summary>
        /// 从vpk文件生成临时文件
        /// </summary>
        private static string GetVPKTemporayPath(string vpk)
        {
            return Path.Combine(Path.GetTempPath(), "L4dModManagerTool", Path.GetFileNameWithoutExtension(vpk));
        }

        /// <summary>
        /// 安全地载入图片，如果图片不存在或有错返回maybe.none
        /// </summary>
        private static Utility.Maybe<Image> LoadImageSafe(string file)
        {
            try
            {
                return Image.FromFile(file);
            }
            catch
            {
                return Utility.Maybe.None;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem i in listView1.SelectedItems)
            {
                // 取出自定义数据
                int modId = (int)i.Tag;
                var mod = ModRepository.Instance.FindModById(modId);
                mod.Map(x => {
                    ModOperation.DeactiveMod(x);
                    return 0;
                });
            }
        }
    }
}
