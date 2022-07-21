
namespace L4d2_Mod_Manager_Tool
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_enableMod = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_disableMod = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_showInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_scanModFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_downloadWorkshopInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tolStripMenuItem_settings = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_about = new System.Windows.Forms.ToolStripMenuItem();
            this.widget_ModOverview1 = new L4d2_Mod_Manager_Tool.Widget.Widget_ModOverview();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.widget_FilterMod1 = new L4d2_Mod_Manager_Tool.Widget.Widget_FilterMod();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_addonInfoDownloadStrategy = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(5, 64);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(731, 495);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.TileSize = new System.Drawing.Size(20, 20);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView1_RetrieveVirtualItem);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
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
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_enableMod,
            this.toolStripMenuItem_disableMod,
            this.toolStripMenuItem_showInExplorer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
            // 
            // toolStripMenuItem_enableMod
            // 
            this.toolStripMenuItem_enableMod.Name = "toolStripMenuItem_enableMod";
            this.toolStripMenuItem_enableMod.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem_enableMod.Text = "启用模组";
            this.toolStripMenuItem_enableMod.Click += new System.EventHandler(this.toolStripMenuItem_enableMod_Click);
            // 
            // toolStripMenuItem_disableMod
            // 
            this.toolStripMenuItem_disableMod.Name = "toolStripMenuItem_disableMod";
            this.toolStripMenuItem_disableMod.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem_disableMod.Text = "禁用模组";
            this.toolStripMenuItem_disableMod.Click += new System.EventHandler(this.toolStripMenuItem_disableMod_Click);
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
            this.menuStrip1.Size = new System.Drawing.Size(1113, 25);
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
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.工具ToolStripMenuItem.Text = "工具(&T)";
            // 
            // toolStripMenuItem_refresh
            // 
            this.toolStripMenuItem_refresh.Name = "toolStripMenuItem_refresh";
            this.toolStripMenuItem_refresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItem_refresh.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem_refresh.Text = "刷新模组列表(&R)";
            this.toolStripMenuItem_refresh.Click += new System.EventHandler(this.toolStripMenuItem_refresh_Click);
            // 
            // toolStripMenuItem_scanModFile
            // 
            this.toolStripMenuItem_scanModFile.Name = "toolStripMenuItem_scanModFile";
            this.toolStripMenuItem_scanModFile.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem_scanModFile.Text = "扫描模组文件(&S)";
            this.toolStripMenuItem_scanModFile.Click += new System.EventHandler(this.toolStripMenuItem_scanModFile_Click);
            // 
            // toolStripMenuItem_downloadWorkshopInfo
            // 
            this.toolStripMenuItem_downloadWorkshopInfo.Name = "toolStripMenuItem_downloadWorkshopInfo";
            this.toolStripMenuItem_downloadWorkshopInfo.Size = new System.Drawing.Size(185, 22);
            this.toolStripMenuItem_downloadWorkshopInfo.Text = "下载工坊信息(&D)";
            this.toolStripMenuItem_downloadWorkshopInfo.Click += new System.EventHandler(this.toolStripMenuItem_downloadWorkshopInfo_Click);
            // 
            // tolStripMenuItem_settings
            // 
            this.tolStripMenuItem_settings.Name = "tolStripMenuItem_settings";
            this.tolStripMenuItem_settings.Size = new System.Drawing.Size(185, 22);
            this.tolStripMenuItem_settings.Text = "选项(&O)";
            this.tolStripMenuItem_settings.Click += new System.EventHandler(this.tolStripMenuItem_settings_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_about});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助ToolStripMenuItem.Text = "帮助(&H)";
            // 
            // toolStripMenuItem_about
            // 
            this.toolStripMenuItem_about.Name = "toolStripMenuItem_about";
            this.toolStripMenuItem_about.Size = new System.Drawing.Size(140, 22);
            this.toolStripMenuItem_about.Text = "关于软件(&A)";
            this.toolStripMenuItem_about.Click += new System.EventHandler(this.toolStripMenuItem_about_Click);
            // 
            // widget_ModOverview1
            // 
            this.widget_ModOverview1.Dock = System.Windows.Forms.DockStyle.Left;
            this.widget_ModOverview1.Location = new System.Drawing.Point(0, 25);
            this.widget_ModOverview1.Name = "widget_ModOverview1";
            this.widget_ModOverview1.Padding = new System.Windows.Forms.Padding(5);
            this.widget_ModOverview1.Size = new System.Drawing.Size(367, 564);
            this.widget_ModOverview1.TabIndex = 9;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(367, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 564);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // widget_FilterMod1
            // 
            this.widget_FilterMod1.Dock = System.Windows.Forms.DockStyle.Top;
            this.widget_FilterMod1.Location = new System.Drawing.Point(5, 5);
            this.widget_FilterMod1.Name = "widget_FilterMod1";
            this.widget_FilterMod1.Size = new System.Drawing.Size(731, 59);
            this.widget_FilterMod1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.widget_FilterMod1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(372, 25);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(741, 564);
            this.panel1.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_addonInfoDownloadStrategy});
            this.statusStrip1.Location = new System.Drawing.Point(0, 589);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1113, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_addonInfoDownloadStrategy
            // 
            this.toolStripStatusLabel_addonInfoDownloadStrategy.Name = "toolStripStatusLabel_addonInfoDownloadStrategy";
            this.toolStripStatusLabel_addonInfoDownloadStrategy.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel_addonInfoDownloadStrategy.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.widget_ModOverview1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "求生之路2模组管理工具";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_showInExplorer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_refresh;
        private System.Windows.Forms.ToolStripMenuItem tolStripMenuItem_settings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_scanModFile;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_about;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_downloadWorkshopInfo;
        private Widget.Widget_ModOverview widget_ModOverview1;
        private System.Windows.Forms.Splitter splitter1;
        private Widget.Widget_FilterMod widget_FilterMod1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_enableMod;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_disableMod;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_addonInfoDownloadStrategy;
    }
}

