using L4d2_Mod_Manager_Tool.Domain;
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
    public partial class Widget_TagSelector : UserControl
    {
        public Widget_TagSelector()
        {
            InitializeComponent();
            GenerateTags("Survivors", ModTag.SurvivorsTags);
            GenerateTags("Infected", ModTag.InfectedTags);
            GenerateTags("Game Content", ModTag.GameContentTags);
            GenerateTags("Game Modes", ModTag.GameModesTags);
            GenerateTags("Weapons", ModTag.WeaponsTags);
            GenerateTags("Items", ModTag.ItemsTags);
        }

        private void GenerateTags(string name, string[] tags)
        {
            flowLayoutPanel1.Controls.Add(new Label()
            {
                Text = name
                ,
                Margin = new Padding(0, 5, 0, 0)
                ,
                Font = new Font("黑体", 10, FontStyle.Bold)
            });
            foreach (var tag in tags)
            {
                flowLayoutPanel1.Controls.Add(new CheckBox()
                {
                    Text = tag
                    ,
                    Margin = new Padding(0)
                    ,
                    Padding = new Padding(0)
                });
            }
        }
    }
}
