
namespace L4d2_Mod_Manager
{
    partial class Form_Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.listBox_modFileLocation = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_addLocation = new System.Windows.Forms.Button();
            this.button_RemoveLocation = new System.Windows.Forms.Button();
            this.button_importLocationFromGameFolder = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_confirm);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 151);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 51);
            this.panel1.TabIndex = 0;
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_confirm.Location = new System.Drawing.Point(416, 9);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 30);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "确定";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(497, 9);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 30);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // listBox_modFileLocation
            // 
            this.listBox_modFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_modFileLocation.FormattingEnabled = true;
            this.listBox_modFileLocation.ItemHeight = 17;
            this.listBox_modFileLocation.Location = new System.Drawing.Point(6, 22);
            this.listBox_modFileLocation.Name = "listBox_modFileLocation";
            this.listBox_modFileLocation.Size = new System.Drawing.Size(404, 106);
            this.listBox_modFileLocation.TabIndex = 1;
            this.listBox_modFileLocation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBox_modFileLocation_KeyUp);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 154);
            this.panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBox_modFileLocation);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 136);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模组储存位置管理";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.button_addLocation);
            this.flowLayoutPanel1.Controls.Add(this.button_RemoveLocation);
            this.flowLayoutPanel1.Controls.Add(this.button_importLocationFromGameFolder);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(416, 22);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(138, 108);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // button_addLocation
            // 
            this.button_addLocation.Location = new System.Drawing.Point(3, 3);
            this.button_addLocation.Name = "button_addLocation";
            this.button_addLocation.Size = new System.Drawing.Size(130, 30);
            this.button_addLocation.TabIndex = 4;
            this.button_addLocation.Text = "添加位置";
            this.button_addLocation.UseVisualStyleBackColor = true;
            this.button_addLocation.Click += new System.EventHandler(this.button_addLocation_Click);
            // 
            // button_RemoveLocation
            // 
            this.button_RemoveLocation.Location = new System.Drawing.Point(3, 39);
            this.button_RemoveLocation.Name = "button_RemoveLocation";
            this.button_RemoveLocation.Size = new System.Drawing.Size(130, 30);
            this.button_RemoveLocation.TabIndex = 2;
            this.button_RemoveLocation.Text = "删除位置";
            this.button_RemoveLocation.UseVisualStyleBackColor = true;
            this.button_RemoveLocation.Click += new System.EventHandler(this.button_RemoveLocation_Click);
            // 
            // button_importLocationFromGameFolder
            // 
            this.button_importLocationFromGameFolder.Location = new System.Drawing.Point(3, 75);
            this.button_importLocationFromGameFolder.Name = "button_importLocationFromGameFolder";
            this.button_importLocationFromGameFolder.Size = new System.Drawing.Size(130, 30);
            this.button_importLocationFromGameFolder.TabIndex = 3;
            this.button_importLocationFromGameFolder.Text = "从游戏文件夹导入";
            this.button_importLocationFromGameFolder.UseVisualStyleBackColor = true;
            this.button_importLocationFromGameFolder.Click += new System.EventHandler(this.button_importLocationFromGameFolder_Click);
            // 
            // Form_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 202);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 241);
            this.Name = "Form_Settings";
            this.ShowIcon = false;
            this.Text = "选项";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ListBox listBox_modFileLocation;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_RemoveLocation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button_addLocation;
        private System.Windows.Forms.Button button_importLocationFromGameFolder;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}