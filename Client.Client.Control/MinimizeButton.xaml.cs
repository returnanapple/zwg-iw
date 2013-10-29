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
    public partial class MinimizeButton : UserControl
    {
        public MinimizeButton()
        {
            InitializeComponent();
        }

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
            if (Click != null) { Click(this, new EventArgs()); }
            if (Command == null) { return; }
            if (!Command.CanExecute(CommandParameter)) { return; }
            Command.Execute(CommandParameter);
        }

        #endregion

        #region 触发命令和命令参数

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MinimizeButton), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(MinimizeButton), new PropertyMetadata(null));

        #endregion

        public event EventHandler Click;
    }
}
