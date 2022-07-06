using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Widget
{
    public partial class Widget_ModOverview : UserControl
    {
        public Widget_ModOverview()
        {
            InitializeComponent();
            flowLayoutPanel1.HorizontalScroll.Maximum = 0;
            flowLayoutPanel1.AutoScroll = true;
        }

        public string ModPreview
        {
            set
            {
                pictureBox1.Image = SelectImage(value);
            }
        }
        public string ModName
        {
            set
            {
                label_modName.Text = value;
            }
        }

        public string ModAuthor
        {
            set
            {
                label_author.Text = "作者: " + StringOr(value, "<未知>");
            }
        }

        public string ModCategories
        {
            set
            {
                label_categories.Text = "分类: " + StringOr(value, "<无>");
            }
        }

        public string ModTags
        {
            set
            {
                label_tags.Text = "标签: " + StringOr(value, "<无>");
            }
        }

        public string ModDescript
        {
            set
            {
                label_descript.Text = value;
            }
        }

        private static string StringOr(string str, string defaultVal)
        {
            return string.IsNullOrEmpty(str) ? defaultVal : str;
        }

        /// <summary>
        /// 选择正确的图片，如果图片不存在或空使用空图片
        /// </summary>
        private static Image SelectImage(string img)
        {
            return LoadImageSafe(img).ValueOr(
                    Image.FromFile(@"Resources/no-image.png"));
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
