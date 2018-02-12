using Message_analysis_by_Elfran;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
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
            textBox2.Height = height - 112;
            textBox3.Height = height - 255;
            //textBox1.Height = height - 115;
            //textBox2.Height = height - 115;
            textBox2.Width = width / 2 -25;
            textBox2.Left = width / 2-3 ;
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
          
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            String version = "V" + AssemblyVersion;
            this.Text += version;
            if (update())
            {



            }
            else {
                this.linkLabel1.Links[0].LinkData = "https://github.com/Elfran1025/National_standard_message_analysis/releases";
                
                label4.Text = version;
               



            }



            //using (WebClient wc = new WebClient())
            //{

            //    json = wc.DownloadString(url);
            //}

            //JObject model = JObject.Parse(json);
            //var sex = model["info"]["sex"];
            //this.Form1_Resize(sender,e);
        }

        /// <summary>
        /// 向目标url发送post请求并收到回复
        /// </summary>
        /// <param name="Url">URL</param>
        /// <param name="sendstr">发送信息</param>
        /// <returns></returns>
        //public string SendToTranspond(string Url, string sendstr)
        //{
        //    string Url = "http://106.14.51.20:8080/EvdataAPI/veh/add";
        //    创建一个HTTP请求
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        //    Post请求方式
        //    request.Method = "GET";
        //    内容类型
        //    request.ContentType = "application/json";//"application/x-www-form-urlencoded";
        //    发送请求，获得请求流
        //    using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
        //    {
        //        //将请求参数写入流
        //        streamWriter.Write(sendstr);
        //    }
        //    string result = string.Empty;
        //    获得响应流
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
        //    {
        //        result = streamReader.ReadToEnd();
        //    }
        //    return result;
        //}
        //private void textBox1_Click(object sender, EventArgs e)
        //{
        //    textBox1.Text = " ";
        //2017-08-15  10:23:40,2017-08-15  10:23:39,LPHFA4F6XHY100064,23 23 02 FE 4C 50 48 46 41 34 46 36 58 48 59 31 30 30 30 36 34 01 01 9B 11 08 0F 0A 17 27 01 01 03 02 00 00 00 00 13 88 15 EA 27 24 50 01 00 07 D0 00 00 04 02 00 00 00 00 05 00 06 AF E5 93 01 EA 0A 98 06 01 01 0F 18 01 01 0F 18 01 01 46 01 01 46 07 00 00 00 00 00 00 00 00 00 08 01 01 15 D6 27 10 00 90 00 01 90 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 0F 18 09 01 01 00 26 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 46 80
        //}

        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.KeepAlive = false;
            request.ContentType = "application/JSON";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static string GetJsonValue(string JSON, string keys)
        {
            string[] key = keys.Split(',');
            for (int i = 0; i < key.Length; i++)
            {
                JSON = JSON.TrimStart().TrimEnd();
                if (JSON[0].ToString() == "[")
                {
                    JSON = JSON.Remove(0, 1);
                    JSON = JSON.Remove(JSON.Length - 1, 1);
                }
                JSON = ((JObject)JsonConvert.DeserializeObject(JSON))[key[i]].ToString();
            }
            return JSON;
        }

        //public static string GetKeyValue(string Json, string Keys) {
        //    ulong index =(ulong) Json.IndexOf(Keys + "\": ");
        //    String[] str = new string[1];
        //    str[0] = "\"" + Keys + "\": ";
        //   String[] jstr= Json.Split(str, System.StringSplitOptions.RemoveEmptyEntries);
        //    return Json;
        //}
        public bool update() {
            String AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            String url = "https://api.github.com/repos/Elfran1025/National_standard_message_analysis/releases/latest";
            String msg = HttpGet(url);
            // MessageBox.Show(msg);
            String json = msg;
            String ver = GetJsonValue(json, "tag_name");
            if (!ver.Equals(AssemblyVersion))
            {

                this.linkLabel1.Links[0].LinkData = GetJsonValue(json, "html_url");
                this.linkLabel1.Text = "立即更新";
                this.label5.Visible = true;
                String version = "V" + ver;
                label4.Text = version;
                //this.Text += version;




                MessageBox.Show("检测到新版本" + ver + "，请立即更新");
                System.Diagnostics.Process.Start(GetJsonValue(json, "assets,browser_download_url"));
                System.Diagnostics.Process.Start(GetJsonValue(json, "html_url"));
                return true;    
            }
            return false;
        }


    }
}
