using System;
using System.Xml.Linq;

namespace IWorld.Setting
{
    /// <summary>
    /// 用于继承的xml的操作类
    /// </summary>
    public abstract class XmlOperator
    {
        #region 保护字段

        /// <summary>
        /// 数据存储对象
        /// </summary>
        protected XElement e;

        #endregion

        #region 保护方法

        /// <summary>
        /// 获取字符串类型的值
        /// </summary>
        /// <param name="key">在文件中存储的键</param>
        /// <param name="_default">默认值</param>
        /// <returns>返回字符串类型的值</returns>
        protected string GetStringValue(string key, string _default)
        {
            try
            {
                return e.Element(key).Value;
            }
            catch (Exception)
            {
                return _default;
            }
        }

        /// <summary>
        /// 获取整数类型的值
        /// </summary>
        /// <param name="key">在文件中存储的键</param>
        /// <param name="_default">默认值</param>
        /// <returns>返回整数类型的值</returns>
        protected int GetIntValue(string key, int _default)
        {
            try
            {
                return Convert.ToInt32(e.Element(key).Value);
            }
            catch (Exception)
            {
                return _default;
            }
        }

        /// <summary>
        /// 获取双精度浮点数数类型的值
        /// </summary>
        /// <param name="key">在文件中存储的键</param>
        /// <param name="_default">默认值</param>
        /// <returns>返回双精度浮点数数类型的值</returns>
        protected double GetDoubleValue(string key, double _default)
        {
            try
            {
                return Convert.ToDouble(e.Element(key).Value);
            }
            catch (Exception)
            {
                return _default;
            }
        }

        /// <summary>
        /// 获取布尔类型的值
        /// </summary>
        /// <param name="key">在文件中存储的键</param>
        /// <param name="_default">默认值</param>
        /// <returns>返回布尔类型的值</returns>
        protected bool GetBooleanValue(string key, bool _default)
        {
            try
            {
                return Convert.ToBoolean(e.Element(key).Value);
            }
            catch (Exception)
            {
                return _default;
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">在文件中存储的键</param>
        /// <param name="value">新值</param>
        protected void SetValue(string key, object value)
        {
            e.Element(key).Value = value.ToString();
        }

        #endregion
    }
}
