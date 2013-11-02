using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace IWorld.Shark.Control.Classes
{
    public class BetInfoListConverter : IValueConverter
    {
        List<BetInfo> BetInfoList;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BetInfoList = (List<BetInfo>)value;
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int source = (int)value;
            IconOfJaw tempp = (IconOfJaw)Enum.Parse(typeof(IconOfJaw), (string)parameter, false);
            bool had = BetInfoList.Any(x => x.BetName == tempp);
            if (had)
            {
                BetInfo bi = BetInfoList.Where(x => x.BetName == tempp).First();
                bi.BetValue = source;
            }
            else
            {
                BetInfoList.Add(new BetInfo(tempp, source));
            }
            return BetInfoList;
        }
    }
}
