using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Visifire.Charts;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 图形化报表
    /// </summary>
    public class ReportFigure : Chart
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的图形化报表
        /// </summary>
        private ReportFigure()
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 移除不必要的文本内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveText(object sender, EventArgs e)
        {
            var c = sender as ReportFigure;
            var legend = c.Legends[0];
            var root = legend.Parent as Grid;

            root.Children.RemoveAt(8);
            root.Children.RemoveAt(7);
        }

        /// <summary>
        /// 初始化加载内容
        /// </summary>
        protected override void LoadWm()
        {
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 实例化一个新的图形化报表
        /// </summary>
        /// <returns>返回图形化报表</returns>
        public static ReportFigure CreateChart()
        {
            ReportFigure result = new ReportFigure();
            result.Rendered += result.RemoveText;

            return result;
        }

        #endregion
    }
}
