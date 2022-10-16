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
            this.label_raskName = new System.Windows.Forms.Label();
            this.progressBar_taskProgress = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_raskName
            // 
            this.label_raskName.AutoEllipsis = true;
            this.label_raskName.Location = new System.Drawing.Point(7, 6);
            this.label_raskName.Name = "label_raskName";
            this.label_raskName.Size = new System.Drawing.Size(152, 17);
            this.label_raskName.TabIndex = 0;
            this.label_raskName.Text = "正在进行的任务名称";
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
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(334, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Widget_BackgroundTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progressBar_taskProgress);
            this.Controls.Add(this.label_raskName);
            this.Name = "Widget_BackgroundTask";
            this.Size = new System.Drawing.Size(369, 47);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_raskName;
        private System.Windows.Forms.ProgressBar progressBar_taskProgress;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Button button1;
    }
}
