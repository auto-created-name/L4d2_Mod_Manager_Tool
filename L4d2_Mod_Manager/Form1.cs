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

        /// <summary>
        /// 更新模组列表
        /// </summary>
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

        #region 定义
        //private class TestMessageTask : TaskFramework.IMessageTask
        //{
        //    public string TaskName { get; private set; }
        //    public TestMessageTask(int i)
        //    {
        //        TaskName = $"正在进行任务{i}...";
        //    }
        //
        //    public void DoTask()
        //    {
        //        System.Threading.Thread.Sleep(10);
        //    }
        //}
        private class ExtraModTask : TaskFramework.IMessageTask
        {
            private ModFile mf;
            public string TaskName { get; private set; }
            public ExtraModTask(ModFile mf)
            {
                TaskName = $"正在解压{mf.FilePath}...";
                this.mf = mf;
            }
            public void DoTask()
            {
                var savedMf = ModFileService.SaveModFile(mf);
                var mod = VPKServices.ExtraMod(savedMf);
                ModRepository.Instance.SaveMod(mod);
            }
        }

        private class DownloadWorkshopInfoTask : TaskFramework.IMessageTask
        {
            private Mod mod;
            public string TaskName { get; private set; }
            public DownloadWorkshopInfoTask(Mod mod)
            {
                TaskName = "下载创意工坊信息，VPKID=" + mod.vpkId + "...";
                this.mod = mod;
            }

            public void DoTask()
            {
                var (newMod, succ) = ModOperation.UpdateWorkshopInfo(mod);
                if (succ)
                {
                    newMod = ModOperation.MoveWorkshopPreviewImage(newMod);
                    ModOperation.UpdateMod(newMod);
                }
            }
        }
        #endregion
        #region UI事件
        private void button1_Click(object sender, EventArgs e)
        {
            // 获取模组文件列表
            // 保存数据库（同步）
            // 将文件转换为模组文件（解压vpk），解析Maddoninfo，获取详细信息
            // 入库

            var tasks = VPKServices.ScanAllModFile()
                .Where(mf => !ModFileService.ModFileExists(mf.FilePath))
                .Select(mf => new ExtraModTask(mf));
            new Form_RunningTask("扫描模组", tasks.ToArray()).ShowDialog();

            //ModFileRepository rp = new ModFileRepository();
            //var mfs = VPKServices.ScanAllModFile().Select(mf => rp.SaveModFile(mf)).ToArray();
            //
            //var res = mfs
            //    .Select(x => VPKServices.ExtraMod(x))
            //    .Select(x => ModRepository.Instance.SaveMod(x))
            //    .ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tasks = ModRepository.Instance.GetMods()
                .Where(ModFP.HaveVpkNumber)
                .Where(Utility.FPExtension.Not<Mod>(ModFP.HaveWorkshopInfo))
                .Select(x => new DownloadWorkshopInfoTask(x));
            new Form_RunningTask("下载创意工坊信息", tasks.ToArray()).ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.SelectedItems)
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
        #endregion

        /// <summary>
        /// 选择正确的图片，如果图片不存在或空使用空图片
        /// </summary>
        private static Image SelectImage(string img)
        {
            return LoadImageSafe(img).ValueOr(
                    Image.FromFile(@"C:\Users\Louis\Desktop\no-image.png"));
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
