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
    public partial class SharkShowInfoCtrl : UserControl
    {
        public SharkShowInfoCtrl()
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
                bd.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 70, 213, 221));
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
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SharkShowInfoCtrl), new PropertyMetadata(false, (d, e) =>
            {
                SharkShowInfoCtrl sInfo = d as SharkShowInfoCtrl;
                if (sInfo != null)
                {
                    sInfo.ChangeStatus((bool)e.NewValue);
                }
            }));


        public string AnimalName
        {
            get { return (string)GetValue(AnimalNameProperty); }
            set { SetValue(AnimalNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimalName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimalNameProperty =
            DependencyProperty.Register("AnimalName", typeof(string), typeof(SharkShowInfoCtrl), new PropertyMetadata(""));



        public string AnimalImagePath
        {
            get { return (string)GetValue(AnimalImagePathProperty); }
            set { SetValue(AnimalImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimalImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimalImagePathProperty =
            DependencyProperty.Register("AnimalImagePath", typeof(string), typeof(SharkShowInfoCtrl), new PropertyMetadata(""));
                

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(SharkShowInfoCtrl), new PropertyMetadata(1));
               

        public string Details
        {
            get { return (string)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Details.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailsProperty =
            DependencyProperty.Register("Details", typeof(string), typeof(SharkShowInfoCtrl), new PropertyMetadata(""));

        private void MouseLeftButtonDownHandle(object sender, EventArgs e)
        {
            this.IsSelected = true;
        }                     

    }
}
