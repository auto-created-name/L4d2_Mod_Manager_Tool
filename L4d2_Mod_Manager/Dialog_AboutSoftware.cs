using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool
{
    public partial class Dialog_AboutSoftware : Form
    {
        public Dialog_AboutSoftware()
        {
            InitializeComponent();

            textBox_version.Text = Version;
            textBox_computerName.Text = ComputerName;
            textBox_operatorVersion.Text = OSVersion;
            textBox_clrVersion.Text = ClrVersion;
        }

        private string Version =>
            $"V{Utility.WinformUtility.SoftwareVersion} - build {File.GetLastWriteTime(ExecuteFile):yyMMddHHmm}";

        private string ComputerName
        {
            get
            {
                var name = Environment.MachineName;
                if (Environment.UserDomainName != name) name = Environment.UserDomainName + "\\" + name;
                return name;
            }
        }

        private string ExecuteFile => CurrentAssembly.Location;

        private Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        private string OSVersion => Environment.OSVersion.ToString();

        private string ClrVersion => Environment.Version.ToString();

        private static DateTime GetPe32Time(string fileName)
        {
            int seconds;
            using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read)))
            {
                var bs = br.ReadBytes(2);
                var msg = "非法的PE32文件";
                if (bs.Length != 2) throw new Exception(msg);
                if (bs[0] != 'M' || bs[1] != 'Z') throw new Exception(msg);
                br.BaseStream.Seek(0x3c, SeekOrigin.Begin);
                var offset = br.ReadByte();
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                bs = br.ReadBytes(4);
                if (bs.Length != 4) throw new Exception(msg);
                if (bs[0] != 'P' || bs[1] != 'E' || bs[2] != 0 || bs[3] != 0) throw new Exception(msg);
                bs = br.ReadBytes(4);
                if (bs.Length != 4) throw new Exception(msg);
                seconds = br.ReadInt32();
            }
            return DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).
              AddSeconds(seconds).ToLocalTime();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
