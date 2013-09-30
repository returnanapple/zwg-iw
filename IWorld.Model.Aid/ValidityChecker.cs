using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model.Aid
{
    /// <summary>
    /// 数据合法性的检查者对象
    /// </summary>
    public class ValidityChecker
    {
        #region 数字数据的边际条件

        /// <summary>
        /// 检查下限边际
        /// </summary>
        /// <param name="beau">目标数据</param>
        /// <param name="lower">下限边际</param>
        /// <param name="propertyDescription">属性说明</param>
        /// <param name="canBeEqual">一个布尔值 标识是否允许目标数据等于边际数据</param>
        public static void CheckLower(double beau, double lower, string propertyDescription, bool canBeEqual = false)
        {
            if (canBeEqual)
            {
                if (beau >= lower) { return; }
            }
            else
            {
                if (beau > lower) { return; }
            }
            string judgment = canBeEqual ? "小于" : "小于/等于";
            string message = string.Format("该操作将导致 {0} {1} {2}，操作无效", propertyDescription, judgment, lower);
            throw new Exception(message);
        }

        /// <summary>
        /// 检查下限边际
        /// </summary>
        /// <param name="beau">目标数据</param>
        /// <param name="lower">下限边际</param>
        /// <param name="propertyDescription">属性说明</param>
        /// <param name="canBeEqual">一个布尔值 标识是否允许目标数据等于边际数据</param>
        public static void CheckLower(int beau, int lower, string propertyDescription, bool canBeEqual = false)
        {
            double _beau = (double)beau;
            double _lower = (double)lower;
            CheckLower(_beau, _lower, propertyDescription, canBeEqual);
        }

        /// <summary>
        /// 检查上限边际
        /// </summary>
        /// <param name="beau">目标数据</param>
        /// <param name="gaps">上限边际</param>
        /// <param name="propertyDescription">属性说明</param>
        /// <param name="canBeEqual">一个布尔值 标识是否允许目标数据等于边际数据</param>
        public static void CheckCaps(double beau, double gaps, string propertyDescription, bool canBeEqual = false)
        {
            if (canBeEqual)
            {
                if (beau <= gaps) { return; }
            }
            else
            {
                if (beau < gaps) { return; }
            }
            string judgment = canBeEqual ? "大于" : "大于/等于";
            string message = string.Format("该操作将导致 {0} {1} {2}，操作无效", propertyDescription, judgment, gaps);
            throw new Exception(message);
        }

        /// <summary>
        /// 检查上限边际
        /// </summary>
        /// <param name="beau">目标数据</param>
        /// <param name="gaps">上限边际</param>
        /// <param name="propertyDescription">属性说明</param>
        /// <param name="canBeEqual">一个布尔值 标识是否允许目标数据等于边际数据</param>
        public static void CheckCaps(int beau, int gaps, string propertyDescription, bool canBeEqual = false)
        {
            double _beau = (double)beau;
            double _lower = (double)gaps;
            CheckCaps(_beau, _lower, propertyDescription, canBeEqual);
        }

        #endregion
    }
}
