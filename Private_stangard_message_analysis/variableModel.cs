using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Private_stangard_message_analysis
{
    class VariableModel
    {
        public VariableModel() {

        }
        public int startbyte;//表示该变量在通讯包内的索引起始位置
        public int length;//表示该变量在通讯包内的数据长度
        public int startbit;//表示该变量在通讯包内的索引起始位置
        public int bitlength;//表示该变量在通讯包内的数据长度
        public decimal accuracy;//表示该变量的精确度
        public decimal min;//表示该变量的最小值
        public decimal max;//表示该变量的最大值
        public string unit;//表示该变量的单位值
        public string description;//表示该变量名称
        public string property;//表示该变量的数据类型
        public decimal a;//解析变量用的参数
        public decimal b;//解析变量用的参数
        public decimal c;//解析变量用的参数
        public decimal d;//解析变量用的参数
        public string display; //是否显示
        public string type;    //数据类型
        public string valueTable;//取值表
        public object Val;     //当前的变量值




    }
}
