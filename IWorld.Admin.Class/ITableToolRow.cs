using System.Windows;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 定义表格工具的行
    /// </summary>
    public interface ITableToolRow
    {
        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <returns></returns>
        FrameworkElement GetElement();

        /// <summary>
        /// 获取展示详细信息的子窗口
        /// </summary>
        /// <returns></returns>
        FrameworkElement GetChildWindow();

        /// <summary>
        /// 鼠标悬停时将触发的事件
        /// </summary>
        /// <returns></returns>
        event TableToolBodyHoverDelegate HoverEventHandler;

        /// <summary>
        /// 取消鼠标悬停时将触发的事件
        /// </summary>
        /// <returns></returns>
        event TableToolBodyHoverDelegate UnhoverEventHandler;
    }
}
