using System;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 表格工具的某一行触发/取消鼠标悬停的监视者对象
    /// </summary>
    public class TableToolBodyHoverEventArgs : EventArgs
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 实例化一个新的表格工具的某一行触发/取消鼠标悬停的监视者对象
        /// </summary>
        /// <param name="row">行号</param>
        public TableToolBodyHoverEventArgs(int row)
        {
            this.Row = row;
        }
    }
}
