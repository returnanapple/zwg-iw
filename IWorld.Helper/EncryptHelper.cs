using System.Linq;
using System.Security.Cryptography;

namespace IWorld.Helper
{
    /// <summary>
    /// 用于加密字符串的帮助类
    /// </summary>
    public class EncryptHelper
    {
        #region 静态方法

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要进行md5加密的对象</param>
        /// <returns>返回一个md5加密后的32位字符串</returns>
        public static string EncryptByMd5(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] temp = System.Text.Encoding.Default.GetBytes(input);
            byte[] data = md5.ComputeHash(temp);
            md5.Clear();

            string result = "";
            data.ToList().ForEach(x =>
            {
                result += x.ToString("x").PadLeft(2, '0');
            });
            return result;
        }

        #endregion
    }
}
