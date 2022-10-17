namespace L4d2_Mod_Manager_Tool.Widget
{
    partial class Widget_BackgroundTask
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
            this.label_taskName = new System.Windows.Forms.Label();
            this.progressBar_taskProgress = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_taskName
            // 
            this.label_taskName.AutoEllipsis = true;
            this.label_taskName.Location = new System.Drawing.Point(7, 6);
            this.label_taskName.Name = "label_taskName";
            this.label_taskName.Size = new System.Drawing.Size(152, 17);
            this.label_taskName.TabIndex = 0;
            this.label_taskName.Text = "正在进行的任务名称";
            // 
            // progressBar_taskProgress
            // 
            this.progressBar_taskProgress.Location = new System.Drawing.Point(163, 9);
            this.progressBar_taskProgress.Name = "progressBar_taskProgress";
            this.progressBar_taskProgress.Size = new System.Drawing.Size(152, 10);
            this.progressBar_taskProgress.TabIndex = 1;
            // 
            // label_status
            // 
            this.label_status.AutoEllipsis = true;
            this.label_status.Location = new System.Drawing.Point(7, 23);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(308, 17);
            this.label_status.TabIndex = 2;
            this.label_status.Text = "具体正在进行的任务名称XXXXXXXXXXXXXXXXXXXXXX";
            // 
            // button_cancel
            // 
            this.button_cancel.FlatAppearance.BorderSize = 0;
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cancel.Location = new System.Drawing.Point(334, 10);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(24, 23);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "X";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // Widget_BackgroundTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progressBar_taskProgress);
            this.Controls.Add(this.label_taskName);
            this.Name = "Widget_BackgroundTask";
            this.Size = new System.Drawing.Size(369, 47);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_taskName;
        private System.Windows.Forms.ProgressBar progressBar_taskProgress;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Button button_cancel;
    }
}
