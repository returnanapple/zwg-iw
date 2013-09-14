using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IWorld.Helper
{
    /// <summary>
    /// 用于验证字符串合法性的帮助类
    /// </summary>
    public class TextHelper
    {
        #region 私有字段

        /// <summary>
        /// 正则表达式集
        /// </summary>
        private static Dictionary<Key, Regex> Regs = new Dictionary<Key, Regex> 
        { 
            { Key.Email, new Regex(@"^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$") } ,
            { Key.Password, new Regex(@"^[a-zA-Z0-9_]{4,32}$") } ,
            { Key.Nickname, new Regex(@"^[\u4e00-\u9fa5A-Za-z0-9_]{2,32}$") } ,
            { Key.Name, new Regex(@"^[u4e00-u9fa5]{2,12}$") } ,
            { Key.IDNumber, new Regex(@"\d{15}|\d{18}$") } ,
            { Key.Birthday, new Regex(@"\d{1,2}/\d{1,2}/\d{4}") } ,
        };

        /// <summary>
        /// 错误提示文本集
        /// </summary>
        private static Dictionary<Key, string> ErrorTexts = new Dictionary<Key, string>
        {
            { Key.Email, "请输入正确的邮箱地址" } ,
            { Key.Password, "密码由4-16位半角字符（字母、数字、下划线）组成，区分大小写" } ,
            { Key.Nickname, "可输入2-12位，包括英文、数字和中文" } ,
            { Key.Name, "只能输入2-12位，纯中文" } ,
            { Key.IDNumber, "请输入正确的证件号码" } ,
            { Key.Birthday, "日期格式错误" } ,
        };

        /// <summary>
        /// 空字段提示
        /// </summary>
        private static string EmptyText = "输入项为空";

        #endregion

        #region 静态方法

        /// <summary>
        /// 检查输入的字符串合法性
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="key">检查模式</param>
        public static void Check(string input, Key key)
        {
            if (input == null || input == "") { throw new Exception(EmptyText); }
            bool licit = Regs[key].IsMatch(input);
            if (!licit) { throw new Exception(ErrorTexts[key]); }
        }

        /// <summary>
        /// 去除字符串首位的空格
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>返回已经去除首位空格的字符串</returns>
        public static string EliminateSpaces(string input)
        {
            input = new Regex(@"^\s+|\s+$").Replace(input, "");
            input = new Regex(@"\s+").Replace(input, " ");

            return input;
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="length">所要截取的长度（半角字符）</param>
        /// <returns>返回按照指定长度截取的字符串</returns>
        public static string Interception(string input, int length)
        {
            string endStr = "";
            string temp = input.Substring(0, (input.Length < length + 1) ? input.Length : length + 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);

            #region 统计字符串长度并填充返回的字符串

            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= length - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > length)
                    break;
            }

            if (count <= length)
            {
                outputStr = temp;
                endStr = "";
            }

            #endregion

            outputStr += endStr;

            return outputStr;
        }

        /// <summary>
        /// 去除所有的html标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码</param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }

        #endregion

        #region 内嵌枚举

        /// <summary>
        /// 正则验证的类型
        /// </summary>
        public enum Key
        {
            /// <summary>
            /// 电子邮箱 xxxx@xxx.xxx格式
            /// </summary>
            Email,
            /// <summary>
            /// 密码 6-16位半角字符（字母、数字、下划线）
            /// </summary>
            Password,
            /// <summary>
            /// 昵称 2-12位，包括英文、数字和中文
            /// </summary>
            Nickname,
            /// <summary>
            /// 姓名 2-12位，纯中文
            /// </summary>
            Name,
            /// <summary>
            /// 身份证号 15/18位数字
            /// </summary>
            IDNumber,
            /// <summary>
            /// 生日日期 xxxx-xx-xx格式
            /// </summary>
            Birthday
        }

        #endregion
    }
}
