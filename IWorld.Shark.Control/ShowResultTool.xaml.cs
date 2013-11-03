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

namespace IWorld.Shark.Control
{
    public partial class ShowResultTool : UserControl
    {
        public ShowResultTool()
        {
            InitializeComponent();
        }
        bool canStop;

        public double Turn
        {
            get { return (double)GetValue(TurnProperty); }
            set { SetValue(TurnProperty, value); }
        }
        public static readonly DependencyProperty TurnProperty =
            DependencyProperty.Register("Turn", typeof(double), typeof(ShowResultTool), new PropertyMetadata(0.0, (d, e) =>
            {
            }));

        public bool Closed
        {
            get { return (bool)GetValue(ClosedProperty); }
            set { SetValue(ClosedProperty, value); }
        }
        public static readonly DependencyProperty ClosedProperty =
            DependencyProperty.Register("Closed", typeof(bool), typeof(ShowResultTool), new PropertyMetadata(false, (d, e) =>
            {
                ShowResultTool tempd = (ShowResultTool)d;
                bool tempe = (bool)e.NewValue;
                Storyboard tempStoryboard = (Storyboard)tempd.Resources["StoryboardX"];
                if (tempe)
                {
                    tempStoryboard.Begin();
                }
                else
                {
                    tempStoryboard.Stop();
                    tempd.Turn = tempd.Result;
                }
            }));

        public int Result
        {
            get { return (int)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(int), typeof(ShowResultTool), new PropertyMetadata(0, (d, e) => 
            {
                ShowResultTool tempd = (ShowResultTool)d;
                if (!tempd.Closed)
                {
                    tempd.Turn = tempd.Result;
                }
            }));
    }

    #region 转换器
    public class TurnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double tempv = (double)value;
            double tempp = System.Convert.ToDouble(parameter.ToString());
            tempv = Math.Round(tempv, 0);
            tempp = Math.Round(tempp, 0);
            int intTempv = (int)tempv;
            int intTempp = (int)tempp;
            if (intTempv % 28 == intTempp)
            { return true; }
            else
            { return false; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
