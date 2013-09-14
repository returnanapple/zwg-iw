using System;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 监视站点地图的条件选择按键的点击事件的对象
    /// </summary>
    public class SiteMapTool_ConditionButtonClickedEventArgs : EventArgs
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 实例化一个新的监视站点地图的条件选择按键的点击事件的对象
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="value">值</param>
        public SiteMapTool_ConditionButtonClickedEventArgs(string condition, string value)
        {
            this.Condition = condition;
            this.Value = value;
        }
    }
}
