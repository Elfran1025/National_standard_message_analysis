using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Message_analysis_by_Elfran
{
    class checkMessage
    {
        static int datalength = 0;
        static List<int> errorSite = new List<int>();
        Decimal dec = new Decimal();
        public void Check(string txt, out string sourceText, out string resultText)
        {
            errorSite.Clear();
            List<String> b = new List<string>();
            sourceText = "";

            txt = txt.ToUpper();
            resultText = "";
            if (txt.Contains("23"))
            {
                if (!txt.Contains(" "))
                {
                    for (int i = 2; i < txt.Length; i += 3)
                    {
                        txt = txt.Insert(i, " ");
                    }
                }

                Console.WriteLine(txt);

                try
                {
                    b = Message_Processing(txt);
                    resultText = Head_Analysis(b ,out sourceText);
                }
                catch (Exception ex)
                {

                    resultText += ex;
                }

                //textBox2.Text += ;
            }
            else
            {
                resultText = "报文中不含“23”，该报文格式不对，请检查后重试";
                //MessageBox.Show("报文中不含“23”，该报文格式不对，请检查后重试");
            }
            



        }


        public List<String> Message_Processing(String text)
        {
            String a = text.Replace("\r\n", null);
            a = a.Replace("\n", null);
            //a = a.Replace("-"," ");
            List<String> b = a.Split(' ').ToList();
            b.RemoveAll(it => it.Equals(""));
            Boolean count = false;
            for (int i = 0; i < b.Count; i++)
            {
                if (b[i] == "23" && b[i + 1] == "23")
                {
                    count = true;
                    //c += b[i];
                    //c += '\n';
                    //i = i + 2;
                    break;
                }
                if (count)
                {
                    //c += b[i];
                    //c += "\r\n";
                }
                else
                {
                    if (b[i] == "23" && b[i + 1] != "23")
                    {
                        b.Insert(i + 1, "23");
                        i = i - 1;
                        continue;
                        // i = i - 1;
                        //i = i + 1;
                    }
                    else
                    {
                        b.Remove(b[i]);
                        i--;
                    }
                }
            }
            //String z = "【行号】";
            //String z = "";
            //string num = "";
            //for (int i = 0; i < b.Count; i++)
            //{
               
            //    if (i!=0&&i % 10 == 0)
            //    {
            //        num = Convert.ToString((i / 10) + 1).PadLeft(3, '0');
            //        //if (i == 0) {

            //        //    z = "【" + num + "】" + z;
            //        //}
                   
            //        //z += "\r\n"+"【"+num+"】";
            //        z += "\r\n";
            //    }
            //    z += (b[i] + " ");
            //}
            //sourceText = z;
            return b;
        }
        //报头解析
        public String Head_Analysis(List<String> b,out String reStr)
        {
            List<string> rlist = new List<string>();
            //MessageBox.Show(z);
            String c = "";
            String details = "";
            String t = "";
            int s = 2;
            reStr = "";
            try
            {


                datalength = Convert.ToInt32(b[22] + b[23], 16);
                int b00 = Convert.ToInt32(b[s], 16);
                switch (b00)
                {
                    case 0x01:
                        details = "车辆登入";
                        t = Car_Login(b);
                        break;
                    case 0x02:
                        details = "实时信息上报";
                        t = Information_Submission(b,out reStr);
                        break;
                    case 0x03:
                        details = "补发信息上报";
                        t = Information_Submission(b, out reStr);
                        break;
                    case 0x04:
                        details = "车辆登出";
                        t = Car_Logout(b);
                        break;
                    case 0x05:
                        details = "平台登入";
                        t = Platform_Login(b);
                        break;
                    case 0x06:
                        details = "平台登出";
                        t = Car_Logout(b);
                        break;
                    case 0x07:
                        details = "心跳";
                        break;
                    case 0x08:
                        details = "终端校时";
                        break;
                    default:
                        details = analysisError(s,null);

                        break;
                }
                if (b00 >= 0x09 && b00 <= 0x7f)
                {
                    details = "上行数据系统预留";
                }
                else if (b00 >= 0x80 && b00 <= 0x82)
                {
                    details = "终端数据预留";
                }
                else if (b00 >= 0x83 && b00 <= 0xBF)
                {
                    details = "下行数据系统预留";
                }
                else if (b00 >= 0xC0 && b00 <= 0xFE)
                {
                    details = "平台交换自定义数据";
                }
                c += "命令标识：\t" + details;
                b00 = Convert.ToInt32(b[++s], 16);
                switch (b00)
                {
                    case 0x01:
                        details = "成功";
                        break;
                    case 0x02:
                        details = "错误";
                        break;
                    case 0x03:
                        details = "VIN重复";
                        break;
                    case 0xFE:
                        details = "命令";
                        break;
                    default:
                        details = analysisError(s, null);
                        break;
                }
                c += "\r\n应答标识：\t" + details;
                details = "";

                for (; s < 20;)
                {
                    char vinStr=Convert.ToChar(Convert.ToByte(b[++s], 16));

                    details += (vinStr=='\0')?"(空)":vinStr.ToString();

                }
                c += "\r\n唯一识别码：\t" + details;

                b00 = Convert.ToInt32(b[++s], 16);
                switch (b00)
                {
                    case 0x01:
                        details = "数据不加密";
                        break;
                    case 0x02:
                        details = "RSA加密";
                        break;
                    case 0x03:
                        details = "AES128加密";
                        break;
                    case 0xFE:
                        details = "异常";
                        break;
                    case 0xFF:
                        details = "无效";
                        break;
                    default:
                        details = analysisError(s, null);
                        break;
                }
                c += "\r\n加密方式：\t" + details;
                //datalength = Convert.ToInt32(b[++s] + b[++s], 16);
                ++s;
                ++s;

                details = checkDecThreshold(datalength,0,65531,null);
                c += "\r\n数据单元长度：\t" + details;
            }
            catch (Exception ex)
            {
                int length = datalength + 25;
                if (length >= s - 1)
                {

                }
                else {
                    c += "长度：" + length;
                    c += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }

               

            }
            byte[] buff = new byte[b.Count];
            for (int j = 0; j < b.Count; j++)
            {
                try
                {
                    buff[j] = Convert.ToByte(b[j], 16);

                }
                catch
                {
                    c += "\r\n错误位置" + j;

                }

            }

            byte check = GetBCC(buff, 2, buff.Length - 3);
            try
            {
                if (buff[datalength + 24] == check)
                {
                    c += "\r\n校验状态：" + "\t【成功】";

                }
                else
                {
                    c += "\r\n校验状态：" + "\t【失败】";
                    string str1 = Convert.ToString(buff[datalength + 24], 16).ToUpper();
                    c += "\r\n原始校验码：" + "\thex：" + str1 + "\tbyte：" + buff.Last();
                    string str2 = Convert.ToString(check, 16).ToUpper();
                    c += "\r\n实际校验码：" + "\thex：" + str2 + "\tbyte：" + check;
                    errorSite.Add(datalength + 24);
                }

            }
            catch {
                c += "\r\n校验状态：" + "\t【失败】";
                c += "\r\n获取不到校验位，请检查数据单元长度是否正确";
                errorSite.Add(22);
                errorSite.Add(23);
                errorSite.Add(datalength + 24);
            }

            String reHead = "";
            reHead = "---------数据包头---------"+" "+24+"\r\n";
            for (int i = 0; i <= 23; i++) {
                if (errorSite.Contains(i))
                {
                    reHead += "【" + b[i] + "】 ";

                }
                else
                {
                    reHead += b[i] + " ";

                }

               // reHead += b[i]+" ";
                
            }

            String reBCC = ""; 
            try
            {
                reBCC = "---------BCC校验---------" + " " + 1 + "\r\n";
                if (errorSite.Contains(datalength + 24))
                {
                    reBCC += "【" + b[datalength + 24] + "】 ";

                }
                else
                {
                    reBCC += b[datalength + 24] + " ";

                }


            }
            catch {
                reBCC = "---------BCC校验---------" + " " + 0 + "\r\n";
            }

            reStr = reHead +"\r\n"+ reStr+reBCC;
            return c + t;

        }
        //信息上报
        public String Information_Submission(List<String> b,out String reStr)
        {
            reStr = "";
            String t = "";
            String details = "";
            int x = 30;



            String h = "";
            int s = 30;
            //if (b.Count > s) {


            //}
            String msg = "";
            try
            {
                details = getTime(b);//获取时间
                reStr += "---------数据采集时间---------" + 6+"\r\n";
                for (int i = 24; i < 30; i++) {
                    reStr += b[i]+" ";
                }
                reStr += "\r\n";
                string variable = "";
                
                t += "\r\n数据采集时间：\t" + details;
                for (s = 30; s < b.Count - 1; s++)
                {
                    msg = "";
                    int b00 = Convert.ToInt32(b[s], 16);
                    if (b00 <= 0x09)
                    {
                        string num = "";
                        switch (b00)
                        {
                            case 0x01:
                                h = "整车数据";

                                #region 整车数据
                                int b01 = Convert.ToInt32(b[++s], 16);
                                switch (b01)
                                {
                                    case 0x01:
                                        details = "启动";
                                        break;
                                    case 0x02:
                                        details = "熄火";
                                        break;
                                    case 0x03:
                                        details = "其他";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n车辆状态：\t" + details;
                                b01 = Convert.ToInt32(b[++s], 16);
                                switch (b01)
                                {
                                    case 0x01:
                                        details = "停车充电";
                                        break;
                                    case 0x02:
                                        details = "行驶充电";
                                        break;
                                    case 0x03:
                                        details = "未充电状态";
                                        break;
                                    case 0x04:
                                        details = "充电完成";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n充电状态：\t" + details;
                                b01 = Convert.ToInt32(b[++s], 16);
                                switch (b01)
                                {
                                    case 0x01:
                                        details = "纯电";
                                        break;
                                    case 0x02:
                                        details = "混动";
                                        break;
                                    case 0x03:
                                        details = "燃油";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n运行模式：\t" + details;
                                variable = b[++s] + b[++s];
                                ////variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec,0,220,null);
                                        break;

                                }



                                msg += "\r\n车速：\t\t" + details;
                                variable = b[++s] + b[++s] + b[++s] + b[++s];
                                ////variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFFFFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFFFFFF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToDecimal(Convert.ToUInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec, 0, new Decimal(999999.9),null);
                                        
                                        break;

                                }


                                msg += "\r\n累计里程：\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec, 0, 1000,null);
                                       
                                        break;

                                }

                                msg += "\r\n总电压：\t\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:

                                        Decimal d = Convert.ToDecimal(Convert.ToInt32(variable, 16));
                                        //Decimal e = 0.1M;
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M-1000;
                                        details = checkDecThreshold(dec, -1000, 1000,null);
                                        
                                        break;

                                }

                                msg += "\r\n总电流：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16));
                                        details = checkDecThreshold(dec, 0, 100,"%");
                                        
                                        break;

                                }

                                msg += "\r\nSOC：\t\t" + details;
                                b01 = Convert.ToInt32(b[++s], 16);
                                switch (b01)
                                {
                                    case 0x01:
                                        details = "工作";
                                        break;
                                    case 0x02:
                                        details = "断开";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\nDC-DC状态：\t" + details;
                                details = Convert.ToString(Convert.ToInt32(b[++s], 16), 2);
                                String temp;
                                String a = "";
                                for (int i = 0; i < 8 - details.Length; i++)
                                {
                                    a += "0";
                                }
                                temp = string.Concat(a, details);
                                //temp = "01234567";
                                switch (temp.ElementAt(3))
                                {
                                    case '1':
                                        details = "有制动力";
                                        break;
                                    case '0':
                                        details = "无制动力";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n制动：\t\t" + details;
                                switch (temp.ElementAt(2))
                                {
                                    case '1':
                                        details = "有驱动力";
                                        break;
                                    case '0':
                                        details = "无驱动力";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n驱动：\t\t" + details;

                                details = temp.Remove(0, 4);
                                int u = Convert.ToInt32(details, 2);//二进制转换成十进制
                                if (u >= 1 && u <= 12)
                                {
                                    details = u + "挡";
                                }
                                else
                                {
                                    switch (u)
                                    {
                                        case 0:
                                            details = "空挡";
                                            break;
                                        case 13:
                                            details = "倒挡";
                                            break;
                                        case 14:
                                            details = "自动D挡";
                                            break;
                                        case 15:
                                            details = "停车P挡";
                                            break;
                                        default:
                                            details = analysisError(s, null);
                                            break;
                                    }
                                }
                                msg += "\r\n档位：\t\t" + details;
                                dec = Convert.ToInt32(b[++s] + b[++s], 16);
                                details = checkDecThreshold(dec,0,60000,null);
                                msg += "\r\n绝缘电阻：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16));
                                        details = checkDecThreshold(dec, 0, 100, "%");
                                        
                                        break;

                                }

                                msg += "\r\n加速踏板行程值：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;
                                    case ("00"):
                                        details = "制动关闭";
                                        break;
                                    case ("65"):
                                        details = "制动有效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16));
                                        details = checkDecThreshold(dec, 1, 100, "%");
                                       
                                        break;

                                }

                                msg += "\r\n制动踏板状态：\t" + details;


                                #endregion

                                break;
                            case 0x02:
                                h = "驱动电机数据";
                                #region 驱动电机数据

                                int b02 = Convert.ToInt32(b[++s], 16);
                                details = checkDecThreshold(b02, 1, 253, null);
                                msg += "\r\n驱动电机个数：\t\t" + details;
                                for (int i = 0; i < b02; i++)
                                {
                                    dec = Convert.ToInt32(b[++s], 16);
                                    details = checkDecThreshold(dec, 1, 253, null);
                                    msg += "\r\n驱动电机序号：\t\t" + details;
                                    int b2 = Convert.ToInt32(b[++s], 16);
                                    switch (b2)
                                    {
                                        case 0x01:
                                            details = "耗电";
                                            break;
                                        case 0x02:
                                            details = "发电";
                                            break;
                                        case 0x03:
                                            details = "关闭状态";
                                            break;
                                        case 0x04:
                                            details = "准备状态";
                                            break;
                                        case 0xFE:
                                            details = "异常";
                                            break;
                                        case 0xff:
                                            details = "无效";
                                            break;
                                        default:
                                            details = analysisError(s, null);
                                            break;
                                    }
                                    msg += "\r\n驱动电机状态：\t\t" + details;
                                    variable = b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FE"):
                                            details = "异常";

                                            break;
                                        case ("FF"):
                                            details = "无效";
                                            break;


                                        default:
                                            dec = Convert.ToInt32(variable, 16) - 40;
                                            details = checkDecThreshold(dec, -40, 210, null);

                                            break;

                                    }

                                    msg += "\r\n驱动电机控制器温度：\t" + details;
                                    variable = b[++s] + b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FFFE"):
                                            details = "异常";

                                            break;
                                        case ("FFFF"):
                                            details = "无效";
                                            break;


                                        default:
                                            dec = Convert.ToInt32(variable, 16) - 20000;
                                            details = checkDecThreshold(dec, -20000, 45531, null);
                                           
                                            break;

                                    }

                                    msg += "\r\n驱动电机转速：\t\t" + details;
                                    variable = b[++s] + b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FFFE"):
                                            details = "异常";

                                            break;
                                        case ("FFFF"):
                                            details = "无效";
                                            break;


                                        default:
                                            dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M - 2000;
                                            details = checkDecThreshold(dec, -2000,new Decimal(4553.1), null);
                                            
                                            break;

                                    }





                                    msg += "\r\n驱动电机转矩：\t\t" + details;
                                    variable = b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FE"):
                                            details = "异常";

                                            break;
                                        case ("FF"):
                                            details = "无效";
                                            break;


                                        default:
                                            dec = Convert.ToInt32(variable, 16) - 40;
                                            details = checkDecThreshold(dec, -40, 210, null);
                                           
                                            break;

                                    }



                                    msg += "\r\n驱动电机温度：\t\t" + details;
                                    variable = b[++s] + b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FFFE"):
                                            details = "异常";

                                            break;
                                        case ("FFFF"):
                                            details = "无效";
                                            break;


                                        default:

                                            dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                            details = checkDecThreshold(dec, 0, 60000, null);
                                            
                                            break;

                                    }



                                    msg += "\r\n电机控制器输入电压：\t" + details;
                                    variable = b[++s] + b[++s];
                                    //variable = variable.ToUpper();
                                    switch (variable)
                                    {
                                        case ("FFFE"):
                                            details = "异常";

                                            break;
                                        case ("FFFF"):
                                            details = "无效";
                                            break;


                                        default:

                                            dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M - 1000;
                                            details = checkDecThreshold(dec, -1000, 1000, null);

                                          
                                            break;

                                    }






                                    msg += "\r\n电机控制器直流母线电流：\t" + details;
                                }

                                #endregion
                                break;
                            case 0x03:
                                h = "燃料电池数据";
                                #region 燃料电池数据
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec, 0, 2000, null);

                                        
                                        break;

                                }






                                msg += "\r\n燃料电池电压：\t\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec, 0, 2000, null);
                                       
                                        break;

                                }



                                msg += "\r\n燃料电池电流：\t\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.01M;
                                        details = checkDecThreshold(dec, 0, 600, null);
                                      
                                        break;

                                }



                                msg += "\r\n燃料消耗率：\t\t" + details;

                                int b03 = Convert.ToInt32(b[++s] + b[++s], 16);
                                details = checkDecThreshold(b03, 0, 65531, null);
                                msg += "\r\n燃料电池温度探针总数：\t" + details;
                                for (int i = 0; i < b03; i++)
                                {
                                    dec = Convert.ToInt32(b[++s], 16) - 40;
                                    details = checkDecThreshold(dec, -40, 200, null);
                                    
                                    msg += "\r\n探针" + (i + 1) + "温度值：\t\t" + details;
                                }

                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M - 40;
                                        details = checkDecThreshold(dec,-40, 200, null);
                                        
                                        break;

                                }





                                msg += "\r\n氢系统中最高温度：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 252, null);
                                        
                                        break;

                                }



                                msg += "\r\n氢系统中最高温度探针代号：\t" + details;


                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 0, 50000, null);

                                       
                                        break;

                                }


                                msg += "\r\n氢气最高浓度：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 252, null);
                                      
                                        break;

                                }



                                msg += "\r\n氢气最高浓度传感器代号：\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                        details = checkDecThreshold(dec, 0, 100, null);
                                        
                                        break;

                                }



                                msg += "\r\n氢气最高压力：\t\t" + details;

                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 252, null);
                                        
                                        break;

                                }


                                msg += "\r\n氢气最高压力传感器代号：\t" + details;

                                b03 = Convert.ToInt32(b[++s], 16);
                                switch (b03)
                                {
                                    case 0x01:
                                        details = "工作";
                                        break;
                                    case 0x02:
                                        details = "断开";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n高压DC/DC状态：\t\t" + details;
                                #endregion
                                break;
                            case 0x04:
                                h = "发动机数据";
                                #region 发动机数据
                                int b04 = Convert.ToInt32(b[++s], 16);
                                switch (b04)
                                {
                                    case 0x01:
                                        details = "启动状态";
                                        break;
                                    case 0x02:
                                        details = "关闭状态";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n发动机状态：\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 0, 60000, null);
                                        
                                        break;

                                }




                                msg += "\r\n曲轴转速：\t" + details;

                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.01M;
                                        details = checkDecThreshold(dec, 0, 600, null);
                                        
                                        break;

                                }


                                msg += "\r\n燃料消耗率：\t" + details;
                                #endregion
                                break;
                            case 0x05:
                                h = "车辆位置数据";
                                #region 车辆位置数据
                                details = Convert.ToString(Convert.ToInt32(b[++s], 16), 2);
                                a = "";
                                for (int i = 0; i < 8 - details.Length; i++)
                                {
                                    a += "0";
                                }
                                temp = string.Concat(a, details);
                                switch (temp.ElementAt(0))
                                {
                                    case '1':
                                        details = "无效定位";
                                        break;
                                    case '0':
                                        details = "有效定位";
                                        break;
                                    default:
                                        details = analysisError(s, null);
                                        break;
                                }
                                msg += "\r\n定位状态：\t" + details;

                                switch (temp.ElementAt(2))
                                {
                                    case '1':
                                        details = "西经";
                                        break;
                                    case '0':
                                        details = "东经";
                                        break;
                                    default:
                                       details = analysisError(s,null);
                                        
                                        break;
                                }
                                double longitude = Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16) * 0.000001;
                                details += Convert.ToString(longitude);

                                msg += "\r\n经度：\t\t" + details;

                                switch (temp.ElementAt(1))
                                {
                                    case '1':
                                        details = "南纬";
                                        break;
                                    case '0':
                                        details = "北纬";
                                        break;
                                    default:
                                        details = analysisError(s,null);
                                        
                                        break;
                                }
                                double latitude = Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16) * 0.000001;
                                details += Convert.ToString(latitude);
                                msg += "\r\n纬度：\t\t" + details;
                                #endregion
                                //msg += "\r\n位置： " + s;
                                break;
                            case 0x06:
                                h = "极值数据";
                                #region 极值数据
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        
                                        break;

                                }




                                msg += "\r\n最高电压电池子系统号：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }


                                msg += "\r\n最高电压电池单体代号：\t" + details;


                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.001M;
                                        details = checkDecThreshold(dec, 0, 15, null);
                                        
                                        break;

                                }

                                msg += "\r\n电池单体电压最高值：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }

                                msg += "\r\n最低电压电池子系统号：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }
                                msg += "\r\n最低电压电池单体代号：\t" + details;
                                variable = b[++s] + b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FFFE"):
                                        details = "异常";

                                        break;
                                    case ("FFFF"):
                                        details = "无效";
                                        break;


                                    default:


                                        dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.001M;
                                        details = checkDecThreshold(dec, 0, 15, null);
                                        break;

                                }


                                msg += "\r\n电池单体电压最低值：\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }



                                msg += "\r\n最高温度子系统号：\t\t" + details;

                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }
                                msg += "\r\n最高温度探针序号：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:
                                        dec = Convert.ToInt32(variable, 16) - 40;
                                        details = checkDecThreshold(dec, -40, 210, null);
                                        
                                        break;

                                }
                                msg += "\r\n最高温度值：\t\t" + details;

                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }
                                msg += "\r\n最低温度子系统号：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16);
                                        details = checkDecThreshold(dec, 1, 250, null);
                                        break;

                                }
                                msg += "\r\n最低温度探针序号：\t\t" + details;
                                variable = b[++s];
                                //variable = variable.ToUpper();
                                switch (variable)
                                {
                                    case ("FE"):
                                        details = "异常";

                                        break;
                                    case ("FF"):
                                        details = "无效";
                                        break;


                                    default:

                                        dec = Convert.ToInt32(variable, 16) - 40;
                                        details = checkDecThreshold(dec, -40, 210, null);
                                        break;

                                }
                                msg += "\r\n最低温度值：\t\t" + details;
                                #endregion
                                break;
                            case 0x07:
                                h = "报警数据";
                                #region 报警数据
                                int b07 = Convert.ToInt32(b[++s], 16);
                                switch (b07)
                                {
                                    case 0x00:
                                        details = "无故障";
                                        break;
                                    case 0x01:
                                        details = "1级故障";
                                        break;
                                    case 0x02:
                                        details = "2级故障";
                                        break;
                                    case 0x03:
                                        details = "3级故障";
                                        break;
                                    case 0xFE:
                                        details = "异常";
                                        break;
                                    case 0xFF:
                                        details = "无效";
                                        break;
                                    default:
                                           details = analysisError(s,null);
                               
                                        break;
                                }
                                msg += "\r\n最高报警等级：\t\t" + details;
                                msg += "\r\n\t\t         通用报警标志";
                                details = Convert.ToString(Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16), 2);
                                a = "";
                                for (int i = 0; i < 32 - details.Length; i++)
                                {
                                    a += "0";
                                }
                                temp = string.Concat(a, details);

                                String[] detail = new String[32];
                                for (int i = 0; i < detail.Length; i++)
                                {
                                    switch (temp.ElementAt(i))
                                    {
                                        case '1':
                                            detail[31 - i] = "【报警】";
                                            break;
                                        case '0':
                                            detail[31 - i] = "正常";
                                            break;
                                        default:
                                            detail[31 - i] = analysisError(s, null);
                                       
                                            break;
                                    }
                                }
                                int j = 0;

                                msg += "\r\n温度差异报警：\t\t" + detail[j];

                                msg += "\r\n电池高温报警：\t\t" + detail[++j];
                                msg += "\r\n车载储能装置类型过压报警：\t" + detail[++j];
                                msg += "\r\n车载储能装置类型欠压报警：\t" + detail[++j];
                                msg += "\r\nSOC低报警：\t\t" + detail[++j];
                                msg += "\r\n单体电池过压报警：\t\t" + detail[++j];
                                msg += "\r\n单体电池欠压报警：\t\t" + detail[++j];
                                msg += "\r\nSOC过高报警：\t\t" + detail[++j];
                                msg += "\r\nSOC跳变报警：\t\t" + detail[++j];
                                msg += "\r\n可充电储能系统不匹配报警：\t" + detail[++j];
                                msg += "\r\n电池单体一致性差报警：\t" + detail[++j];
                                msg += "\r\n绝缘报警：\t\t" + detail[++j];
                                msg += "\r\nDC-DC温度报警：\t\t" + detail[++j];
                                msg += "\r\n制动系统报警：\t\t" + detail[++j];
                                msg += "\r\nDC-DC状态报警：\t\t" + detail[++j];
                                msg += "\r\n驱动电机控制器温度报警：\t" + detail[++j];
                                msg += "\r\n高压互锁状态报警：\t\t" + detail[++j];
                                msg += "\r\n驱动电机温度报警：\t\t" + detail[++j];
                                msg += "\r\n车载储能装置类型过充报警：\t" + detail[++j];
                                //msg += "\r\n位置： " + s;
                                b07 = Convert.ToInt32(b[++s], 16);
                                details = checkDecThreshold(b07, 0, 252, null);
                                msg += "\r\n可充电储能装置故障总数：\t" + details;
                                for (int i = 0; i < b07; i++)
                                {
                                    details = Convert.ToString(Convert.ToInt64(b[++s] + b[++s] + b[++s] + b[++s], 16));
                                    msg += "\r\n可充电储能装置故障代码列表：\t\t" + details;
                                }
                                b07 = Convert.ToInt32(b[++s], 16);
                                details = checkDecThreshold(b07, 0, 252, null);
                                msg += "\r\n驱动电机故障总数：\t\t" + details;
                                for (int i = 0; i < b07; i++)
                                {
                                    details = Convert.ToString(Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16));
                                    msg += "\r\n驱动电机故障代码列表：\t\t" + details;
                                }
                                b07 = Convert.ToInt32(b[++s], 16);
                                details = checkDecThreshold(b07, 0, 252, null);
                                msg += "\r\n发动机故障总数：\t\t" + details;
                                for (int i = 0; i < b07; i++)
                                {
                                    details = Convert.ToString(Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16));
                                    msg += "\r\n发动机故障代码列表：\t\t" + details;
                                }
                                b07 = Convert.ToInt32(b[++s], 16);
                                details = checkDecThreshold(b07, 0, 252, null);
                                msg += "\r\n其他故障总数：\t\t" + details;
                                for (int i = 0; i < b07; i++)
                                {
                                    details = Convert.ToString(Convert.ToInt32(b[++s] + b[++s] + b[++s] + b[++s], 16));
                                    msg += "\r\n其他故障代码列表：\t\t" + details;
                                }

                                #endregion

                                //msg += "\r\n位置： " + s;
                                break;
                            case 0x08:



                                h = "可充电储能装置电压数据";

                                #region MyRegion


                                int b08 = Convert.ToInt32(b[++s], 16);

                                if (b08 == 254)
                                {
                                    details = "异常";
                                }
                                else if (b08 == 255)
                                {
                                    details = "无效";
                                }
                                else {
                                   
                                    details = checkDecThreshold(b08, 1, 250, null);
                                }
                                msg += "\r\n可充电储能子系统个数：\t" + details;
                                if (b08 > 253) {

                                    break;
                                }

                                else
                                {
                                    for (int i = 0; i < b08; i++)
                                    {
                                        try
                                        {
                                            dec = Convert.ToInt32(b[++s], 16);
                                            details = checkDecThreshold(dec, 1, 250, null);
                                            msg += "\r\n可充电储能子系统号：\t" + details;

                                            variable = b[++s] + b[++s];
                                            //variable = variable.ToUpper();
                                            switch (variable)
                                            {
                                                case ("FFFE"):
                                                    details = "异常";

                                                    break;
                                                case ("FFFF"):
                                                    details = "无效";
                                                    break;


                                                default:
                                                    dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M;
                                                    details = checkDecThreshold(dec, 0, 1000, null);
                                                   
                                                    break;

                                            }



                                            msg += "\r\n可充电储能装置电压：\t" + details;

                                            variable = b[++s] + b[++s];
                                            //variable = variable.ToUpper();
                                            switch (variable)
                                            {
                                                case ("FFFE"):
                                                    details = "异常";

                                                    break;
                                                case ("FFFF"):
                                                    details = "无效";
                                                    break;


                                                default:
                                                    dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.1M - 1000;
                                                    details = checkDecThreshold(dec, -1000, 1000, null);
                                                    
                                                    break;

                                            }
                                            msg += "\r\n可充电储能装置电流：\t" + details;


                                            variable = b[++s] + b[++s];
                                            //variable = variable.ToUpper();
                                            switch (variable)
                                            {
                                                case ("FFFE"):
                                                    details = "异常";

                                                    break;
                                                case ("FFFF"):
                                                    details = "无效";
                                                    break;


                                                default:
                                                    dec = Convert.ToInt32(variable, 16);
                                                    details = checkDecThreshold(dec, 1, 65531, null);
                                                  
                                                    break;

                                            }

                                            msg += "\r\n单体电池总数：\t\t" + details;

                                            int b801 = Convert.ToInt32(b[++s] + b[++s], 16);
                                            details = checkDecThreshold(b801, 1, 65531, null);
                                            msg += "\r\n本帧起始电池序号：\t\t" + details;

                                            int b8 = Convert.ToInt32(b[++s], 16);
                                            details = checkDecThreshold(b8, 1, 200, null);
                                            msg += "\r\n本帧单体电池总数：\t\t" + details;
                                            msg += "\r\n";
                                            num = "";
                                            msg += "\t\t         单体电池电压\r\n";
                                            for (int k = 0; k < b8; k++)
                                            {
                                                try
                                                {

                                                    variable = b[++s] + b[++s];
                                                    //variable = variable.ToUpper();
                                                    switch (variable)
                                                    {
                                                        case ("FFFE"):
                                                            details = "异常";

                                                            break;
                                                        case ("FFFF"):
                                                            details = "无效";
                                                            break;


                                                        default:
                                                            dec = Convert.ToDecimal(Convert.ToInt32(variable, 16)) * 0.001M;
                                                            details = checkDecThreshold(dec, 0, 60, null).PadRight(5, '0'); ;
                                                           
                                                            break;

                                                    }

                                                    num = (b801++).ToString().PadLeft(3, '0');
                                                    msg += ("电压" + num + "：" + details) + "\t";
                                                    if ((k + 1) % 3 == 0 && (k + 1 != b8))
                                                    {

                                                        msg += "\r\n";
                                                    }

                                                }
                                                catch {
                                                    msg +="\r\n"+ analysisError(s, "本帧单体电池总数多于实际数据");
                                                    //msg += "\r\n解析错误，本帧单体电池总数多于实际数据";
                                                    //msg += "\r\n位置：" + s;
                                                    break;
                                                }


                                            }

                                        }
                                        catch {
                                            msg += "\r\n" + analysisError(s, "可充电储能子系统个数多于实际数据");
                                            //msg += "\r\n解析错误，可充电储能子系统个数多于实际数据";
                                            //msg += "\r\n位置：" + s;
                                            break;
                                        }

                                    }


                                }
                                #endregion

                                break;



                            case 0x09:
                                h = "可充电储能装置温度数据";
                                #region MyRegion


                                int b09 = Convert.ToInt32(b[++s], 16);
                                if (b09 == 254)
                                {
                                    details = "异常";
                                }
                                else if (b09 == 255)
                                {
                                    details = "无效";
                                }
                                else
                                {
                                    
                                    details = checkDecThreshold(b09, 1, 250, null);
                                    
                                }




                                //details = Convert.ToString(b09);
                                msg += "\r\n可充电储能子系统个数：\t" + details;
                                num = "";


                                if (b09 > 253)
                                {
                                    break;
                                }
                                else {
                                    for (int i = 0; i < b09; i++)
                                    {
                                        try {
                                            dec = Convert.ToInt32(b[++s], 16);
                                            details = checkDecThreshold(dec, 1, 250, null);
                                            
                                            msg += "\r\n可充电储能子系统号：\t" + details;

                                            int b9 = Convert.ToInt32(b[++s] + b[++s], 16);
                                            details = checkDecThreshold(b9, 1, 65531, null);

                                            msg += "\r\n可充电储能温度探针个数：\t" + details;
                                            msg += "\r\n";
                                            msg += "\t\t         单体温度\r\n";
                                            for (int k = 0; k < b9; k++)
                                            {
                                                try
                                                {
                                                    variable = b[++s];
                                                    //variable = variable.ToUpper();
                                                    switch (variable)
                                                    {
                                                        case ("FE"):
                                                            details = "异常";

                                                            break;
                                                        case ("FF"):
                                                            details = "无效";
                                                            break;


                                                        default:
                                                            dec = Convert.ToInt32(variable, 16) - 40;
                                                            details = checkDecThreshold(dec, -40, 210, null);
                                                            ;
                                                            break;

                                                    }

                                                    num = (k + 1).ToString().PadLeft(3, '0');
                                                    msg += "温度" + num + "：" + details + "    ";
                                                    if ((k + 1) % 4 == 0 && (k + 1) != b9)
                                                    {

                                                        msg += "\r\n";
                                                    }


                                                }
                                                catch {
                                                    msg += "\r\n" + analysisError(s, "可充电储能温度探针个数多于实际数据");
                                                    //msg += "\r\n解析错误，可充电储能温度探针个数多于实际数据";
                                                    //msg += "\r\n位置：" + s;
                                                    break;

                                                }

                                            }



                                        }
                                        catch {
                                            msg += "\r\n" + analysisError(s, "可充电储能子系统个数多于实际数据");
                                            //msg += "\r\n解析错误，可充电储能子系统个数多于实际数据";
                                            //msg += "\r\n位置：" + s;
                                            break;
                                        }


                                    }


                                }
                                #endregion

                                break;




                            default:
                                h = "【解析错误】";
                                msg += "\r\n位置： " + s;
                                s = b.Count - 1;
                                break;
                        }


                    }

                    else if (b00 >= 0x0A && b00 <= 0x2F)
                    {
                        h = "平台交换协议自定义数据";
                        int length;
                        variable = b[++s] + b[++s];
                        ////variable = variable.ToUpper();
                        length = Convert.ToInt32(variable, 16);
                        details = length.ToString();



                        msg += "\r\n自定义数据长度：\t\t" + details;
                        s += length;


                        //s = b.Count - 1;
                    }
                    else if (b00 >= 0x30 && b00 <= 0x7F)
                    {
                        h = "预留";
                        s = b.Count - 1;
                    }
                    else if (b00 >= 0x80 && b00 <= 0xFE)
                    {
                        h = "终端数据预留";
                        int length;
                        variable = b[++s] + b[++s];
                        ////variable = variable.ToUpper();
                        length = Convert.ToInt32(variable, 16);
                        details = length.ToString();
                        msg += "\r\n自定义数据长度：\t\t" + details;
                        s += length;
                    }
                    else
                    {
                        h = "【解析错误】";

                        msg += "\r\n位置： " + s;
                        s = b.Count - 1;

                    }

                    reStr += "---------" + h + "--------- " + (s - x + 1) + "\r\n";
                    if (s > b.Count()) {

                        s = b.Count()-1;
                    }
                    for (int i = x; i <= s; i++)
                    {
                        if (errorSite.Contains(i))
                        {
                            reStr += "【" + b[i] + "】 ";

                        }
                        else
                        {
                            reStr += b[i] + " ";

                        }


                    }
                    reStr += "\r\n";
                    x = s + 1;

                    h = "\r\n信息类型标志：\t---------" + h + "---------";

                    t += h + msg;

                    //msg += h + t;
                }
                //details = b[++s];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);





                reStr += "\r\n---------" + h + "--------- " + (s - x + 1) + "\r\n";

                for (int i = x; i < b.Count(); i++)
                {
                    if (errorSite.Contains(i))
                    {

                        reStr += "【" + b[i] + "】 ";

                    }
                    else
                    {
                        reStr += b[i] + " ";

                    }


                }
                reStr += "\r\n";
                x = s + 1;

                h = "\r\n信息类型标志：\t---------" + h + "---------";

                t += h + msg;


                int length = datalength + 24;
                if (length == s - 1)
                {

                }
                else if (length > s - 1)
                {
                    t += "\r\n实际长度小于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;
                }

                else
                {
                    t += "\r\n实际长度大于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;




                    //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }

                //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
            }
            finally {




            }


            return t;
        }
        //车辆登入
        public String Car_Login(List<String> b)
        {
            String t = "";
            String details = "";
            int s = 29;
            try
            {




                details = getTime(b);
                t += "\r\n数据采集时间：\t" + details;
                dec = Convert.ToInt32(b[++s] + b[++s], 16);
                details = checkDecThreshold(dec, 0, 65531,null);
                t += "\r\n登入流水号：\t" + details;
                details = "";
                for (int i = 0; i < 20; i++)
                {
                    details += Convert.ToChar(Convert.ToByte(b[++s], 16));
                }
                t += "\r\nICCID：\t\t" + details;
                int n = Convert.ToInt32(b[++s], 16);

                details = checkDecThreshold(n, 0, 250,null);
                t += "\r\n可充电储能子系统数：\t" + details;
                int m = Convert.ToInt32(b[++s], 16);
                details = checkDecThreshold(m,0,50,null);
                t += "\r\n可充电储能系统编码长度：\t" + details;
                details = "";
                for (int i = 0; i < m * n; i++)
                {
                    details += Convert.ToChar(Convert.ToByte(b[++s], 16));

                }
                t += "\r\n可充电储能系统编码：\t" + details;
            }
            catch (Exception ex)
            {
                int length = datalength + 24;
                if (length == s - 1)
                {

                }
                else if (length > s - 1)
                {
                    t += "\r\n实际长度小于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;
                }

                else
                {
                    t += "\r\n实际长度大于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;



                    //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }

            }
            return t;
        }
        //车辆登出、平台登出
        public String Car_Logout(List<String> b)
        {
            String t = "";
            String details = "";
            int s = 29;
            try
            {
                details = getTime(b);
                t += "\r\n数据采集时间：\t" + details;
                dec = Convert.ToInt32(b[++s] + b[++s], 16);
                details = checkDecThreshold(dec, 0, 65531, null);
                
                t += "\r\n登出流水号：\t" + details;

            }
            catch (Exception ex)
            {

                int length = datalength + 24;
                if (length == s - 1)
                {

                }
                else if (length > s - 1)
                {
                    t += "\r\n实际长度小于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;
                }

                else
                {
                    t += "\r\n实际长度大于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;




                    //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }
                //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
            }

            return t;
        }
        //平台登入
        public String Platform_Login(List<String> b)
        {
            String t = "";
            String details = "";
            int s = 29;
            try
            {
                details = getTime(b);
                t += "\r\n数据采集时间：\t" + details;

                dec = Convert.ToInt32(b[++s] + b[++s], 16);
                details = checkDecThreshold(dec, 0, 65531, null);
                t += "\r\n登入流水号：\t" + details;
                details = "";
                for (int i = 0; i < 12; i++)
                {
                    details += Convert.ToChar(Convert.ToByte(b[++s], 16));
                }
                t += "\r\n平台用户名：\t" + details;
                details = "";
                for (int i = 0; i < 20; i++)
                {
                    details += Convert.ToChar(Convert.ToByte(b[++s], 16));
                }
                t += "\r\n平台密码：\t" + details;

                int b00 = Convert.ToInt32(b[++s], 16);
                switch (b00)
                {
                    case 0x01:
                        details = "数据不加密";
                        break;
                    case 0x02:
                        details = "RSA加密";
                        break;
                    case 0x03:
                        details = "AES128加密";
                        break;
                    case 0xFE:
                        details = "异常";
                        break;
                    case 0xFF:
                        details = "无效";
                        break;
                    default:
                           details = analysisError(s,null);
 
                        break;
                }
                t += "\r\n加密方式：\t" + details;


            }
            catch (Exception ex)
            {
                int length = datalength + 24;
                if (length == s - 1)
                {

                }
                else if (length > s - 1)
                {
                    t += "\r\n实际长度小于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;
                }

                else
                {
                    t += "\r\n实际长度大于数据单元长度";
                    t += "\r\n数据单元长度" + datalength;
                    t += "\r\n实际长度" + (s - 25);
                    t += "\r\n位置：" + s;




                    //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }
                //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;


            }



            return t;


        }
        //获取时间
        public String getTime(List<String> b)
        {
            String t = "";
            int s = 24;
            try
            {
                String yy = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2,'0');
                String MM = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2, '0');
                String dd = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2, '0');
                String hh = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2, '0');
                String mm = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2, '0');
                String ss = Convert.ToString(Convert.ToInt32(b[s++], 16)).PadLeft(2, '0');
                //if (!yy.Equals("00"))
                //{
                //    yy = "20" + yy;
                //}
                //else {

                //}
                t = yy + "年" + MM + "月" + dd + "日" + hh + "时" + mm + "分" + ss + "秒";
            }
            catch (Exception ex)
            {
                int length = datalength + 25;
                if (length >= s - 1)
                {

                }
                else
                {
                    t += "长度：" + length;
                    t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;
                }
                //t += "\r\n位置：" + s + "\r\n异常：" + ex.Message;

            }

            return t;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>

        
        public byte GetBCC(byte[] buffer, int offset, int len)
        {
            byte value = buffer[offset];
            for (int i = offset + 1; i < offset + len; i++)
            {
                value = (byte)(value ^ buffer[i]);
            }
            return value;
        }
        public string analysisError(int s,String detail) {

            String str = "【解析错误】 "+detail;
            str+= "\r\n位置： " + s;
            errorSite.Add(s);
            return str;
        }

        public string checkIntThreshold(int value, int min, int max) {
           String detail= Convert.ToString(value);
            if (value < min || value > max) {

                detail += "【超出阈值】";

            }
            return detail;
        }
        public String checkDecThreshold(Decimal value, Decimal min, Decimal max,String unit ) {
            String detail = Convert.ToString(value)+unit;
            if (value < min || value > max)
            {

                detail += "【超出阈值】";

            }
            return detail;

        }



    }



}
