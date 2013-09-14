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

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 技术支持按键
    /// </summary>
    public partial class TechnicalSupport_Button : UserControl
    {
        public TechnicalSupport_Button()
        {
            InitializeComponent();
            root.MouseEnter += ShowHoverStyle;
            root.MouseLeave += ShowNormailStyle;
            root.MouseLeftButtonDown += ShowTriggerStyle;
            root.MouseLeftButtonUp += ShowHoverStyle;
            root.MouseLeftButtonUp += Click;
        }

        #region 样式

        void ShowHoverStyle(object sender, EventArgs e)
        {
            root.Style = (Style)this.Resources["horver"];
        }

        void ShowNormailStyle(object sender, EventArgs e)
        {
            root.Style = (Style)this.Resources["normal"];
        }

        void ShowTriggerStyle(object sender, EventArgs e)
        {
            root.Style = (Style)this.Resources["trigger"];
        }

        #endregion

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TechnicalSupport_Button), new PropertyMetadata(null));

        /// <summary>
        /// 触发命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Click(object sender, EventArgs e)
        {
            if (Command == null)
            {
                return;
            }
            if (!Command.CanExecute(null))
            {
                return;
            }
            Command.Execute(null);
        }
    }
}
