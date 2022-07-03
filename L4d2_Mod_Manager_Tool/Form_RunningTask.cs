using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L4d2_Mod_Manager_Tool.TaskFramework;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Form_RunningTask : Form
    {
        private IMessageTask[] tasks;
        public Form_RunningTask(string taskName, IMessageTask[] tasks)
        {
            InitializeComponent();
            // 设置标题
            Text = taskName;
            this.tasks = tasks;

            // 开始任务
            backgroundWorker1.RunWorkerAsync();
        }
        private void AppendMessageLine(string msg)
        {
            //stringBuilder.AppendLine(msg);
            //textBox_msg.Text = stringBuilder.ToString();
            textBox_msg.AppendText(msg + Environment.NewLine);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker)sender;

            if (tasks == null || tasks.Length == 0)
            {
                bgWorker.ReportProgress(100);
                return;
            }

            for (int i = 0; i < tasks.Length; ++i)
            {
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    var curTask = tasks[i];

                    try
                    {
                        // 在多线程中调用其他UI会引起错误
                        //AppendMessageLine(curTask.TaskName);

                        curTask.DoTask();
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    // 将消息通过ReportProgress转发出去
                    int prog = Math.Clamp((int)((i + 1.0f) / tasks.Length * 100), 0, 100);
                    string msg = $"({i + 1}/{tasks.Length}){curTask.TaskName}";
                    bgWorker.ReportProgress(prog, msg);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            AppendMessageLine((string)e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                AppendMessageLine("任务被取消");
                progressBar1.Value = 100;
            }
            else
            {
                AppendMessageLine("任务已完成");
            }

            button_cancel.Enabled = false;
            button_close.Enabled = true;
        }

        private void Form_RunningTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                var res = MessageBox.Show("有任务正在进行，是否取消任务？", "中断任务", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                }
                e.Cancel = true;
            }
        }
    }
}
