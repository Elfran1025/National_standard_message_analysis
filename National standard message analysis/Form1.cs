using Message_analysis_by_Elfran;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace National_standard_message_analysis
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            //textBox2.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text) && textBox1.Text == "")
            {
                string str = (String)iData.GetData(DataFormats.Text);
                textBox1.Text = str;
            }
            String txt = textBox1.Text.Replace("-",null);
            string sourceText="";
            string resultText = "";
            checkMessage cm = new checkMessage();
            
            cm.Check(txt,out sourceText,out resultText);
        
            textBox3.Text = sourceText;
            textBox2.Text = resultText+ "\r\n■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■\r\n" + textBox2.Text;
            //richTextBox1.Text = resultText + "\r\n■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■\r\n" + richTextBox1;
            //public string Replace(string oldValue, string newValue)
        }

        //报文处理：字符串处理，将String转换为List




 
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //richTextBox1.Clear();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = this.Width;
            int height = this.Height;
            textBox1.Width = width / 2 - 25;
            textBox3.Width = textBox1.Width;
            //textBox1.Height = height - 115;
            //textBox2.Height = height - 115;
            textBox2.Width = width / 2 -30;
            textBox2.Left = width / 2 ;
            label2.Left = textBox2.Left;
            button2.Left = textBox2.Left;
            button1.Left = textBox1.Right - 75;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button1.Text = "解析";
            }
            else {
                button1.Text = "粘贴并解析";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[0].LinkData = "https://github.com/Elfran1025/National_standard_message_analysis/releases";
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           String version = "V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label4.Text = version;
            this.Text += version;
            //this.Form1_Resize(sender,e);
        }


        //private void textBox1_Click(object sender, EventArgs e)
        //{
        //    textBox1.Text = " ";
        //2017-08-15  10:23:40,2017-08-15  10:23:39,LPHFA4F6XHY100064,23 23 02 FE 4C 50 48 46 41 34 46 36 58 48 59 31 30 30 30 36 34 01 01 9B 11 08 0F 0A 17 27 01 01 03 02 00 00 00 00 13 88 15 EA 27 24 50 01 00 07 D0 00 00 04 02 00 00 00 00 05 00 06 AF E5 93 01 EA 0A 98 06 01 01 0F 18 01 01 0F 18 01 01 46 01 01 46 07 00 00 00 00 00 00 00 00 00 08 01 01 15 D6 27 10 00 90 00 01 90 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 09 01 01 00 26 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 80
        //}
    }
}
