
namespace L4d2_Mod_Manager_Tool.Widget
{
    partial class Widget_ModOverview
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label_modName = new System.Windows.Forms.Label();
            this.label_author = new System.Windows.Forms.Label();
            this.label_categories = new System.Windows.Forms.Label();
            this.label_tags = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_descript = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(509, 206);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(521, 218);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 218);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(521, 373);
            this.panel2.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Controls.Add(this.label_modName);
            this.flowLayoutPanel1.Controls.Add(this.label_author);
            this.flowLayoutPanel1.Controls.Add(this.label_categories);
            this.flowLayoutPanel1.Controls.Add(this.label_tags);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.label_descript);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(509, 361);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label_modName
            // 
            this.label_modName.AutoSize = true;
            this.label_modName.Font = new System.Drawing.Font("黑体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_modName.Location = new System.Drawing.Point(13, 10);
            this.label_modName.Name = "label_modName";
            this.label_modName.Size = new System.Drawing.Size(92, 22);
            this.label_modName.TabIndex = 0;
            this.label_modName.Text = "Mod名称";
            // 
            // label_author
            // 
            this.label_author.AutoSize = true;
            this.label_author.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_author.Location = new System.Drawing.Point(13, 52);
            this.label_author.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label_author.Name = "label_author";
            this.label_author.Size = new System.Drawing.Size(39, 17);
            this.label_author.TabIndex = 1;
            this.label_author.Text = "作者: ";
            // 
            // label_categories
            // 
            this.label_categories.AutoSize = true;
            this.label_categories.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_categories.Location = new System.Drawing.Point(13, 89);
            this.label_categories.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label_categories.Name = "label_categories";
            this.label_categories.Size = new System.Drawing.Size(39, 17);
            this.label_categories.TabIndex = 2;
            this.label_categories.Text = "分类: ";
            // 
            // label_tags
            // 
            this.label_tags.AutoSize = true;
            this.label_tags.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_tags.Location = new System.Drawing.Point(13, 126);
            this.label_tags.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label_tags.Name = "label_tags";
            this.label_tags.Size = new System.Drawing.Size(39, 17);
            this.label_tags.TabIndex = 5;
            this.label_tags.Text = "标签: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(13, 163);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "描述: ";
            // 
            // label_descript
            // 
            this.label_descript.AutoSize = true;
            this.label_descript.Location = new System.Drawing.Point(13, 180);
            this.label_descript.Name = "label_descript";
            this.label_descript.Size = new System.Drawing.Size(480, 51);
            this.label_descript.TabIndex = 4;
            this.label_descript.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" +
    "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 218);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(521, 5);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // Widget_ModOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Widget_ModOverview";
            this.Size = new System.Drawing.Size(521, 591);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label_author;
        private System.Windows.Forms.Label label_categories;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_descript;
        private System.Windows.Forms.Label label_modName;
        private System.Windows.Forms.Label label_tags;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
    }
}
