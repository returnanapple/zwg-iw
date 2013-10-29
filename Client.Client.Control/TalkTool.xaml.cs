using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Client.Client.Control
{
    public partial class TalkTool : UserControl
    {
        public TalkTool()
        {
            InitializeComponent();
            BingSomthing();
        }

        #region 依赖属性

        public bool CanSee
        {
            get { return (bool)GetValue(CanSeeProperty); }
            set { SetValue(CanSeeProperty, value); }
        }

        public static readonly DependencyProperty CanSeeProperty =
            DependencyProperty.Register("CanSee", typeof(bool), typeof(TalkTool)
            , new PropertyMetadata(true, (d, e) =>
            {
                TalkTool tool = (TalkTool)d;
                bool canSee = (bool)e.NewValue;
                tool.Visibility = canSee ? Visibility.Visible : Visibility.Collapsed;
                if (canSee) { return; }
                if (tool.ClearChatCommand == null) { return; }
                if (!tool.ClearChatCommand.CanExecute(null)) { return; }
                tool.ClearChatCommand.Execute(null);
            }));

        public ICommand ClearChatCommand
        {
            get { return (ICommand)GetValue(ClearChatCommandProperty); }
            set { SetValue(ClearChatCommandProperty, value); }
        }

        public static readonly DependencyProperty ClearChatCommandProperty =
            DependencyProperty.Register("ClearChatCommand", typeof(ICommand), typeof(TalkTool), new PropertyMetadata(null));

        #endregion

        #region 依赖属性

        public double ShowHeight
        {
            get { return (double)GetValue(ShowHeightProperty); }
            set { SetValue(ShowHeightProperty, value); }
        }

        public static readonly DependencyProperty ShowHeightProperty =
            DependencyProperty.Register("ShowHeight", typeof(double), typeof(TalkTool)
            , new PropertyMetadata(0.0, (d, e) =>
            {
                TalkTool tool = (TalkTool)d;
                tool.sv.ScrollToVerticalOffset((double)e.NewValue);
            }));

        #endregion

        #region 私有方法

        void BingSomthing()
        {
            Binding binding = new Binding("ExtentHeight") { Source = this.sv };
            this.SetBinding(TalkTool.ShowHeightProperty, binding);
        }

        #endregion
    }
}
