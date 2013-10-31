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

namespace IWorld.Client
{
    public partial class SharkRotateItemCtrl : UserControl
    {
        public SharkRotateItemCtrl()
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
                image.Effect = effect;
                //bd.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 70, 213, 221));
            }
            else
            {
                //bd.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 70, 213, 221));
                image.Effect = null;
            }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SharkRotateItemCtrl), new PropertyMetadata(false, (d, e) =>
            {
                SharkRotateItemCtrl sInfo = d as SharkRotateItemCtrl;
                if (sInfo != null)
                {
                    sInfo.ChangeStatus((bool)e.NewValue);
                }
            }));

        
        public string RotateItemImagePath
        {
            get { return (string)GetValue(RotateItemImagePathProperty); }
            set { SetValue(RotateItemImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RotateItemImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotateItemImagePathProperty =
            DependencyProperty.Register("RotateItemImagePath", typeof(string), typeof(SharkRotateItemCtrl), new PropertyMetadata(""));

   

        public int RotateItemID { get; set; }

        public string RotateItemName { get; set; }
    }
}
