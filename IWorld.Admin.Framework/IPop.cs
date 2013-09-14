using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 定义弹窗
    /// </summary>
    public interface IPop
    {
        /// <summary>
        /// 触发弹窗的消息
        /// </summary>
        IMessage Message { get; set; }

        /// <summary>
        /// 获取监听条件
        /// </summary>
        /// <returns>返回监听条件</returns>
        List<MonitorCondition> GetMonitorConditions();

        /// <summary>
        /// 显示
        /// </summary>
        void Show();
    }
}
