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
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IWorld.Client
{
    public partial class SharkInfoTipCtrl : UserControl
    {
        public SharkInfoTipCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 改变选中状态
        /// </summary>
        /// <param name="bIsSelect"></param>
        void ChangeStatus(bool bIsSelect)
        {
            if (bIsSelect)
            {
                DropShadowEffect effect = (DropShadowEffect)this.Resources["selectEffect"];
                bd.Effect = effect;
                bd.BorderBrush = new SolidColorBrush(Color.FromArgb(255,70,213,221));
            }
            else
            {
                bd.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 70, 213, 221));
                bd.Effect = null;
            }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SharkInfoTipCtrl), new PropertyMetadata(false, (d, e) =>
                {
                    SharkInfoTipCtrl tip = d as SharkInfoTipCtrl;
                    if (tip != null)
                    {
                        tip.ChangeStatus((bool)e.NewValue);
                    }
                }));


        public string TipTitle
        {
            get { return (string)GetValue(TipTitleProperty); }
            set { SetValue(TipTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TipTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipTitleProperty =
            DependencyProperty.Register("TipTitle", typeof(string), typeof(SharkInfoTipCtrl), new PropertyMetadata(""));



        public string TipContnet
        {
            get { return (string)GetValue(TipContnetProperty); }
            set { SetValue(TipContnetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TipContnet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipContnetProperty =
            DependencyProperty.Register("TipContnet", typeof(string), typeof(SharkInfoTipCtrl), new PropertyMetadata(""));



        
    }
}
