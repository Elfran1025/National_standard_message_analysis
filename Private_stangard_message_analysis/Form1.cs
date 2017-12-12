using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Private_stangard_message_analysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            OpenFileDialog file1 = new OpenFileDialog();
            if (file1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = File.OpenText(file1.FileName);
                string path = System.IO.Path.GetFullPath(file1.FileName);
                textBox1.Text = path;
                //String line;
                //while ((line = sr.ReadLine()) != null)
                //{
                //    textBox2.Text += (line.ToString()+"\r\n");
                //}


                //while (sr.EndOfStream != true)
                //{

                //    richTextBox1.Text += sr.ReadLine()+"\r\n";
                //    //output();
                //}
                XmlDocument document = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlReader reader = XmlReader.Create(@path, settings);
                try
                {
                    settings.IgnoreComments = true;//忽略文档里面的注释
                    document.Load(reader);
                    String detail = "";
                    XmlNode dataNode = document.SelectSingleNode("Data");
                    if (dataNode != null)
                    {
                        XmlNode variablesNode = dataNode.SelectSingleNode("variables");
                        if (variablesNode != null)
                        {
                            string variablesName = variablesNode.Attributes["name"].Value;
                            string variablesValue = variablesNode.Attributes["value"].Value;
                            detail += "variables" + " " + "value：" + variablesValue + "\t" + "name：" + variablesName + "\r\n";
                            XmlNodeList idNodeList = variablesNode.ChildNodes;
                            if (idNodeList != null)
                            {
                                foreach (XmlNode idNode in idNodeList)
                                {
                                    string idName = idNode.Attributes["name"].Value;
                                    string idValue = idNode.Attributes["value"].Value;
                                    string idLength = idNode.Attributes["length"].Value;
                                    detail += "\t" + "ID：" + " " + "value：" + idValue + "\t" + "name：" + idName + "\t" + "length：" + idLength + "\r\n";
                                    if (idNode != null)
                                    {
                                        XmlNodeList variableNodeList = idNode.ChildNodes;
                                        if (variableNodeList != null)
                                        {
                                            foreach (XmlNode variableNode in variableNodeList)
                                            {
                                                detail += "\t\t" + variableNode.Name + "\r\n";

                                                //            string name = attri.Name;
                                                //            string value = attri.Value;
                                                XmlNodeList modelNodeList = variableNode.ChildNodes;

                                                if (modelNodeList != null)
                                                {
                                                    VariableModel variableModel = new VariableModel();
                                                    variableModel.startbyte = Convert.ToInt32(modelNodeList.Item(0).InnerText);
                                                    variableModel.length = Convert.ToInt32(modelNodeList.Item(1).InnerText);
                                                    variableModel.startbit = Convert.ToInt32(modelNodeList.Item(2).InnerText);
                                                    variableModel.bitlength = Convert.ToInt32(modelNodeList.Item(3).InnerText);
                                                    variableModel.accuracy = Convert.ToDecimal(modelNodeList.Item(4).InnerText);
                                                    variableModel.min = Convert.ToDecimal(modelNodeList.Item(5).InnerText);
                                                    variableModel.max = Convert.ToDecimal(modelNodeList.Item(6).InnerText);
                                                    variableModel.unit = modelNodeList.Item(7).InnerText;
                                                    variableModel.description = modelNodeList.Item(8).InnerText;
                                                    variableModel.property = modelNodeList.Item(9).InnerText;
                                                    variableModel.a = Convert.ToDecimal(modelNodeList.Item(10).InnerText);
                                                    variableModel.b = Convert.ToDecimal(modelNodeList.Item(11).InnerText);
                                                    variableModel.c = Convert.ToDecimal(modelNodeList.Item(12).InnerText);
                                                    variableModel.d = Convert.ToDecimal(modelNodeList.Item(13).InnerText);
                                                    variableModel.display = modelNodeList.Item(14).InnerText;
                                                    variableModel.type = modelNodeList.Item(15).InnerText;
                                                    variableModel.valueTable = modelNodeList.Item(16).InnerText;
                                                    PropertyInfo[] propertys = variableModel.GetType().GetProperties();
                                                    int i = 0;
                                                    //foreach (PropertyInfo pinfo in propertys)
                                                    //{
                                                    //    //detail+="<br>" + pinfo.Name + "," + pinfo.GetValue(myobj, null);
                                                    //    detail +="对象"+  pinfo.Name + "," + modelNodeList.Item(i).InnerText;
                                                    //    i++;
                                                    //}



                                                    foreach (XmlNode modelNode in modelNodeList)
                                                    {
                                                        String modelName = modelNode.Name;
                                                        String modelText = modelNode.InnerText;
                                                        detail += "\t\t\t"  + modelName + "：\t" + modelText + "\r\n";

                                                    }


                                                }
                                                //detail += "\t\t"  + variableNode.Name  + "\r\n";



                                            }

                                        }




                                    }




                                }




                            }



                        }

                    }
                    textBox2.Text = detail;
                    //string innerXmlInfo = xn.InnerXml.ToString();
                    //string outerXmlInfo = xn.OuterXml.ToString();
                    //XmlNodeList xnl = xn.ChildNodes;
                    //foreach (XmlNode node in xnl)
                    //{
                    //    XmlAttributeCollection attributeCol = node.Attributes;
                    //    foreach (XmlAttribute attri in attributeCol)
                    //    {
                    //        //获取属性名称与属性值

                    //        textBox2.Text+= name+"\t"+value+"\r\n";
                    //    }

                    //    if (node.HasChildNodes)
                    //    {
                    //        XmlAttributeCollection attributeCol2 = node.NextSibling;
                    //        ////获取该节点的第一个子节点
                    //        //XmlNode secondLevelNode1 = node.FirstChild;
                    //        ////获取该节点的名字
                    //        //string name = secondLevelNode1.Name;
                    //        ////获取该节点的值（即：InnerText）
                    //        //string innerText = secondLevelNode1.InnerText;
                    //        //textBox2.Text += name + "\t" + innerText+"\r\n";
                    //        foreach (XmlAttribute attri in xnl2)
                    //        {
                    //            //获取属性名称与属性值
                    //            string name = attri.Name;
                    //            string value = attri.Value;
                    //            textBox2.Text += name + "\t" + value + "\r\n";
                    //        }

                    //        //for (int i = 0; i < node.ChildNodes.Count; i++) {


                    //        //    //获取该节点的第二个子节点（用数组下标获取）
                    //        //    XmlNode secondLevelNode2 = node.ChildNodes[1];
                    //        //    String name = secondLevelNode2.Name;
                    //        //    String value = secondLevelNode2.Value;
                    //        //    String innerText = secondLevelNode2.InnerText;
                    //        //    textBox2.Text += name + "\t"+value +"\t"+ innerText + "\r\n";
                    //        //}
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    textBox2.Text += ex.ToString();
                }
                finally
                {
                    reader.Close();
                }
            }

            //xnl.
            //XElement shuxing = xn.Element("name");
            //Console.WriteLine(shuxing.Value);
        }



    }
}
