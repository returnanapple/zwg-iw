using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 按键按下所要触发的动作
    /// </summary>
    public class KeyDownCommandAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// 触发动作
        /// </summary>
        /// <param name="parameter">参数</param>
        protected override void Invoke(object parameter)
        {
            KeyEventArgs e = (KeyEventArgs)parameter;
            if (e.Key != Key)
            {
                return;
            }
            if (Command != null)
            {
                Command.Execute(parameter);
            }
        }

        #region 命令

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyDownCommandAction), new PropertyMetadata(null));

        #endregion

        #region 用于触发动作的按键

        /// <summary>
        /// 用于触发动作的按键
        /// </summary>
        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Key), typeof(KeyDownCommandAction), new PropertyMetadata(Key.Enter));

        #endregion
    }
}
