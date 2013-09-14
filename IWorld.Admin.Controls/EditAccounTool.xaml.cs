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
    public partial class EditAccounTool : ChildWindow, IPop
    {
        public EditAccounTool()
        {
            InitializeComponent();
        }

        #region 消息

        /// <summary>
        /// 消息
        /// </summary>
        public IMessage Message { get; set; }

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
        public List<MonitorCondition> GetMonitorConditions()
        {
            List<MonitorCondition> conditions = new List<MonitorCondition>();
            conditions.Add(new MonitorCondition(ComAction.EditAccount, EditAccountActionStatus.WriteInformation));

            return conditions;
        }

        /// <summary>
        /// 反馈弹窗操作结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void ShowResult(object sender, EventArgs e)
        {
            EditAccounTool tool = (EditAccounTool)sender;
            if (tool.DialogResult != true)
            {
                return;
            }
            if (tool.Message == null)
            {
                tool.DialogResult = false;
                return;
            }
            IEditAccountPackage package = new EditAccountPackage
            {
                OldPassword = input_oldPassword.Password,
                NewPassword = input_newPassword.Password,
                NewPassword_Confirm = inpu_newPassword_confirm.Password
            };
            tool.Message.Handle(package);
            Messager.Default.Send(tool.Message);
        }

        #region 内嵌类型

        /// <summary>
        /// 修改编辑用户信息的参数的封装
        /// </summary>
        class EditAccountPackage : IEditAccountPackage
        {
            /// <summary>
            /// 原密码
            /// </summary>
            public string OldPassword { get; set; }

            /// <summary>
            /// 新密码
            /// </summary>
            public string NewPassword { get; set; }

            /// <summary>
            /// 新密码确认
            /// </summary>
            public string NewPassword_Confirm { get; set; }
        }

        #endregion
    }
}

