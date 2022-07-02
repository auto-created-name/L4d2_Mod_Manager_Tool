
namespace L4d2_Mod_Manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.button_clearFilter = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_showInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_scanModFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_downloadWorkshopInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tolStripMenuItem_settings = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_about = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(12, 36);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(776, 377);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名称";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "作者";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "简介";
            this.columnHeader4.Width = 300;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(20, 20);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textBox_search
            // 
            this.textBox_search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_search.Location = new System.Drawing.Point(12, 7);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.PlaceholderText = "搜索模组名称、VPKID、作者...";
            this.textBox_search.Size = new System.Drawing.Size(776, 23);
            this.textBox_search.TabIndex = 5;
            this.textBox_search.TextChanged += new System.EventHandler(this.textBox_search_TextChanged);
            // 
            // button_clearFilter
            // 
            this.button_clearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearFilter.BackColor = System.Drawing.Color.White;
            this.button_clearFilter.FlatAppearance.BorderSize = 0;
            this.button_clearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_clearFilter.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_clearFilter.Location = new System.Drawing.Point(764, 5);
            this.button_clearFilter.Name = "button_clearFilter";
            this.button_clearFilter.Size = new System.Drawing.Size(23, 21);
            this.button_clearFilter.TabIndex = 6;
            this.button_clearFilter.Text = "X";
            this.button_clearFilter.UseVisualStyleBackColor = false;
            this.button_clearFilter.Click += new System.EventHandler(this.button_clearFilter_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_showInExplorer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 26);
            // 
            // toolStripMenuItem_showInExplorer
            // 
            this.toolStripMenuItem_showInExplorer.Name = "toolStripMenuItem_showInExplorer";
            this.toolStripMenuItem_showInExplorer.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem_showInExplorer.Text = "在文件管理器中显示";
            this.toolStripMenuItem_showInExplorer.Click += new System.EventHandler(this.toolStripMenuItem_showInExplorer_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工具ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_refresh,
            this.toolStripMenuItem_scanModFile,
            this.toolStripMenuItem_downloadWorkshopInfo,
            this.tolStripMenuItem_settings});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // toolStripMenuItem_refresh
            // 
            this.toolStripMenuItem_refresh.Name = "toolStripMenuItem_refresh";
            this.toolStripMenuItem_refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItem_refresh.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem_refresh.Text = "刷新模组列表";
            this.toolStripMenuItem_refresh.Click += new System.EventHandler(this.toolStripMenuItem_refresh_Click);
            // 
            // toolStripMenuItem_scanModFile
            // 
            this.toolStripMenuItem_scanModFile.Name = "toolStripMenuItem_scanModFile";
            this.toolStripMenuItem_scanModFile.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem_scanModFile.Text = "扫描模组文件";
            this.toolStripMenuItem_scanModFile.Click += new System.EventHandler(this.toolStripMenuItem_scanModFile_Click);
            // 
            // toolStripMenuItem_downloadWorkshopInfo
            // 
            this.toolStripMenuItem_downloadWorkshopInfo.Name = "toolStripMenuItem_downloadWorkshopInfo";
            this.toolStripMenuItem_downloadWorkshopInfo.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem_downloadWorkshopInfo.Text = "下载工坊信息";
            this.toolStripMenuItem_downloadWorkshopInfo.Click += new System.EventHandler(this.toolStripMenuItem_downloadWorkshopInfo_Click);
            // 
            // tolStripMenuItem_settings
            // 
            this.tolStripMenuItem_settings.Name = "tolStripMenuItem_settings";
            this.tolStripMenuItem_settings.Size = new System.Drawing.Size(169, 22);
            this.tolStripMenuItem_settings.Text = "设置";
            this.tolStripMenuItem_settings.Click += new System.EventHandler(this.tolStripMenuItem_settings_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_about});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // toolStripMenuItem_about
            // 
            this.toolStripMenuItem_about.Name = "toolStripMenuItem_about";
            this.toolStripMenuItem_about.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem_about.Text = "关于软件";
            this.toolStripMenuItem_about.Click += new System.EventHandler(this.toolStripMenuItem_about_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.textBox_search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 425);
            this.panel1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button_clearFilter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "求生之路2模组管理工具";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.Button button_clearFilter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_showInExplorer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_refresh;
        private System.Windows.Forms.ToolStripMenuItem tolStripMenuItem_settings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_scanModFile;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_about;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_downloadWorkshopInfo;
    }
}

