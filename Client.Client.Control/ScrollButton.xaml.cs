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

namespace Client.Client.Control
{
    public partial class ScrollButton : UserControl
    {
        public ScrollButton()
        {
            InitializeComponent();
        }

        #region 依赖属性

        #region 按键方向

        public TEnum Orientation
        {
            get { return (TEnum)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(TEnum), typeof(ScrollButton)
            , new PropertyMetadata(TEnum.up, (d, e) =>
            {
                ScrollButton tool = (ScrollButton)d;
                TEnum orientation = (TEnum)e.NewValue;
                if (orientation == TEnum.up)
                {
                    tool.line.VerticalAlignment = VerticalAlignment.Bottom;
                    tool.rotation.Rotation = -90;
                }
                else
                {
                    tool.line.VerticalAlignment = VerticalAlignment.Top;
                    tool.rotation.Rotation = 90;
                }
            }));

        #endregion

        #region 触发命令和命令参数

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ScrollButton), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ScrollButton), new PropertyMetadata(null));

        #endregion

        #endregion

        #region 事件

        public event EventHandler Click;

        #endregion

        #region 内嵌枚举

        public enum TEnum
        {
            up,
            dwon
        }

        #endregion

        #region 鼠标事件

        private void OnHover(object sender, MouseEventArgs e)
        {
            bg.Visibility = System.Windows.Visibility.Visible;
        }

        private void OnUnhover(object sender, MouseEventArgs e)
        {
            bg.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            if (Click == null) { return; }
            Click(this, new EventArgs());
            if (Command == null) { return; }
            if (!Command.CanExecute(CommandParameter)) { return; }
            Command.Execute(CommandParameter);
        }

        #endregion
    }
}
