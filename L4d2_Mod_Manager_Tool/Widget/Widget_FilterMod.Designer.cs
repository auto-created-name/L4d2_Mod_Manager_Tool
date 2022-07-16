
namespace L4d2_Mod_Manager_Tool.Widget
{
    partial class Widget_FilterMod
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
            this.button_filter = new System.Windows.Forms.Button();
            this.button_clearFilter = new System.Windows.Forms.Button();
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel_filter = new System.Windows.Forms.FlowLayoutPanel();
            this.button_category = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_filter
            // 
            this.button_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_filter.Location = new System.Drawing.Point(661, -1);
            this.button_filter.Name = "button_filter";
            this.button_filter.Size = new System.Drawing.Size(47, 23);
            this.button_filter.TabIndex = 10;
            this.button_filter.Text = "标签";
            this.button_filter.UseVisualStyleBackColor = true;
            this.button_filter.Click += new System.EventHandler(this.button_filter_Click);
            // 
            // button_clearFilter
            // 
            this.button_clearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearFilter.BackColor = System.Drawing.Color.White;
            this.button_clearFilter.FlatAppearance.BorderSize = 0;
            this.button_clearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_clearFilter.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_clearFilter.Location = new System.Drawing.Point(576, 1);
            this.button_clearFilter.Name = "button_clearFilter";
            this.button_clearFilter.Size = new System.Drawing.Size(23, 21);
            this.button_clearFilter.TabIndex = 9;
            this.button_clearFilter.Text = "X";
            this.button_clearFilter.UseVisualStyleBackColor = false;
            this.button_clearFilter.Click += new System.EventHandler(this.button_clearFilter_Click);
            // 
            // textBox_search
            // 
            this.textBox_search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_search.Location = new System.Drawing.Point(0, 0);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.PlaceholderText = "搜索模组名称、VPKID、作者...";
            this.textBox_search.Size = new System.Drawing.Size(601, 23);
            this.textBox_search.TabIndex = 8;
            this.textBox_search.TextChanged += new System.EventHandler(this.textBox_search_TextChanged);
            // 
            // flowLayoutPanel_filter
            // 
            this.flowLayoutPanel_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel_filter.Location = new System.Drawing.Point(0, 26);
            this.flowLayoutPanel_filter.Name = "flowLayoutPanel_filter";
            this.flowLayoutPanel_filter.Size = new System.Drawing.Size(708, 34);
            this.flowLayoutPanel_filter.TabIndex = 11;
            // 
            // button_category
            // 
            this.button_category.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_category.Location = new System.Drawing.Point(608, -1);
            this.button_category.Name = "button_category";
            this.button_category.Size = new System.Drawing.Size(47, 23);
            this.button_category.TabIndex = 12;
            this.button_category.Text = "内容";
            this.button_category.UseVisualStyleBackColor = true;
            this.button_category.Click += new System.EventHandler(this.button_category_Click);
            // 
            // Widget_FilterMod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_category);
            this.Controls.Add(this.flowLayoutPanel_filter);
            this.Controls.Add(this.button_filter);
            this.Controls.Add(this.button_clearFilter);
            this.Controls.Add(this.textBox_search);
            this.Name = "Widget_FilterMod";
            this.Size = new System.Drawing.Size(708, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_filter;
        private System.Windows.Forms.Button button_clearFilter;
        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_filter;
        private System.Windows.Forms.Button button_category;
    }
}
