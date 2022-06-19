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
        private ModRepository repo = new ModRepository();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var snippets = VPKServices.ScanVPK();
            foreach(var snip in snippets)
            {
                var mod = ModFP.CreateMod(snip.VpkName);

                // 赋值缩略图信息
                mod = snip.AddonImage.Match(img => mod with { Thumbnail = img }, () => mod);
                mod = snip.AddonInfo.Match(info =>
                {
                    var modinfo = ModOperation.ReadModInfo(info);
                    return mod with
                    {
                        Title = modinfo.Title.ValueOr(""),
                        Version = modinfo.Version.ValueOr(""),
                        Tagline = modinfo.Tagline.ValueOr(""),
                        Author = modinfo.Author.ValueOr("")
                    };
                }, () => mod);
                repo.SaveMod(mod);
                //var modInfo = ModOperation.ReadModInfo(snip.AddonInfo);
                //if (File.Exists(snip.AddonImage))
                //    imageList1.Images.Add(snip.VpkName, Image.FromFile(snip.AddonImage));
                //else
                //    imageList1.Images.Add("noimage", Image.FromFile(@"C:\Users\Louis\Desktop\no-image.png"));

                //modInfo.Match(mi => listView1.Items.Add(mi.Title.Match(x => x, () => snip.VpkName), index++),
                //    () => listView1.Items.Add(snip.VpkName, index++)
                //);
                //
                //var mod = modInfo.Bind(mi => Utility.Maybe.Some(new Mod(
                //    snip.VpkName,
                //    snip.AddonImage.ValueOr(""),
                //    mi.Title.ValueOr(""),
                //    mi.Version.ValueOr(""),
                //    mi.Tagline.ValueOr(""),
                //    mi.Author.ValueOr(""),
                //    null, null, null))
                //) ;
                //
                //mod.Match(mod => repo.SaveMod(mod), () => repo.SaveMod(ModFP.CreateMod(snip.VpkName)));
                //if (index > 10) break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateModList(repo.GetMods());
        }

        private void UpdateModList(IEnumerable<Mod> mods)
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();

            int index = 0;

            foreach (var mod in mods)
            {
                var img = SelectImage(mod);
                imageList1.Images.Add(mod.Vpk, img);
                //if (File.Exists(mod.Thumbnail))
                //    imageList1.Images.Add(mod.Vpk, Image.FromFile(mod.Thumbnail));
                //else
                //    imageList1.Images.Add("noimage", Image.FromFile(@"C:\Users\Louis\Desktop\no-image.png"));
                //listView1.Items.Add(ModFP.BestName(mod), index++);
                ListViewItem item = new ListViewItem();
                item.Text = ModFP.SelectName(mod);
                item.ImageIndex = index++;
                listView1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //HttpWebRequest req = 
            //    (HttpWebRequest)WebRequest.Create(
            //        "https://steamcommunity.com/sharedfiles/filedetails/?id=2658557986");
            //var res = req.GetResponse();
            //using var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            //string html = reader.ReadToEnd();

            var a = repo.GetMods()
                .Where(x => !ModFP.HaveWorkshopInfo(x))
                .Select(x => ModOperation.UpdateWorkshopInfo(x))
                .Where(x => x.Item2)
                .Select(x => x.Item1)
                .Select(x => ModOperation.MoveWorkshopPreviewImage(x))
                .ToArray();
            foreach(var b in a)
            {
                repo.UpdateMod(b);
            }
        }

        private static Image SelectImage(Mod mod)
        {
            return LoadImageSafe(mod.WorkshopPreviewImage).ValueOr(() =>
                LoadImageSafe(mod.Thumbnail).ValueOr(
                    Image.FromFile(@"C:\Users\Louis\Desktop\no-image.png")));
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
    }
}
