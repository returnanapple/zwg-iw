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
            Storyboard tempdStoryboard2 = this.Resources["StoryboardX2"] as Storyboard;
            tempdStoryboard2.Completed += (sender, e) => { this.Turn = this.Turn % 28; };
        }

        #region 用于动画
        public double Turn
        {
            get { return (double)GetValue(TurnProperty); }
            set { SetValue(TurnProperty, value); }
        }
        public static readonly DependencyProperty TurnProperty =
            DependencyProperty.Register("Turn", typeof(double), typeof(ShowResultTool), new PropertyMetadata(0.0));
        #endregion

        #region 结果
        public int Result
        {
            get { return (int)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(int), typeof(ShowResultTool), new PropertyMetadata(-3, (d, e) =>
            {
                ShowResultTool tempd = (ShowResultTool)d;
                Storyboard tempdStoryboard = tempd.Resources["StoryboardX"] as Storyboard;
                int tempe = (int)e.NewValue;
                if (tempe >= 0)
                {
                    Storyboard tempdStoryboard2 = tempd.Resources["StoryboardX2"] as Storyboard;
                    DoubleAnimation tempdDoubleAnimation2 = tempd.DoubleAnimationX2;
                    tempdStoryboard.Pause();
                    int offset = 0;
                    double t = 0;
                    int tempTurn = Convert.ToInt32(tempd.Turn.ToString());
                    offset = Math.Abs(tempe - (tempTurn % 28));
                    tempdDoubleAnimation2.By = 56 + offset;
                    t = Math.Round((double)(56 + offset) / 19, 2);
                    tempdDoubleAnimation2.Duration = new Duration(TimeSpan.Parse("0:0:" + t.ToString()));
                    tempdStoryboard2.Begin();
                    tempdStoryboard.Stop();
                }
                else if (tempe == -1)
                {
                    tempdStoryboard.Begin();
                }
            }));
        #endregion
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
