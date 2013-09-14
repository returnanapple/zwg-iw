using System;

namespace IWorld.Helper
{
    /// <summary>
    /// 枚举对象的帮助者对象
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 将字符串转换为枚举对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="input">字符串</param>
        /// <returns>返回指定类型的枚举对象</returns>
        public static T Parse<T>(string input)
        {
            Type type = typeof(T);
            if (!Enum.IsDefined(type, input)) { throw new Exception("指定的枚举对象中并不存在指定的枚举值"); }
            return (T)Enum.Parse(typeof(T), input);
        }
    }
}
