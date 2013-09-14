using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Framework;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 普通提示弹窗
    /// </summary>
    public partial class NormalPrompt : ChildWindow, IPop
    {
        /// <summary>
        /// 实例化一个新的普通提示弹窗
        /// </summary>
        public NormalPrompt()
        {
            InitializeComponent();
        }

        #region 信息

        IMessage message = null;

        /// <summary>
        /// 信息
        /// </summary>
        public IMessage Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                text_content.Text = message.Content.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 获取监听条件
        /// </summary>
        /// <returns>返回监听条件</returns>
        List<MonitorCondition> IPop.GetMonitorConditions()
        {
            List<MonitorCondition> conditions = new List<MonitorCondition>();
            //用户登出
            conditions.Add(new MonitorCondition(ComAction.Logout, LogoutActionStatus.WaitForConfirm));

            return conditions;
        }

        /// <summary>
        /// 反馈弹窗操作结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void ShowResult(object sender, EventArgs e)
        {
            NormalPrompt tool = (NormalPrompt)sender;
            if (tool.DialogResult != true)
            {
                return;
            }
            if (tool.Message == null)
            {
                tool.DialogResult = false;
                return;
            }
            tool.Message.Handle();
            Messager.Default.Send(tool.Message);
        }
    }
}

