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
using IWorld.Shark.Control.JawService;

namespace IWorld.Shark.Control.Classes
{
    public class BetChildWindowConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<BetInfo> source = (List<BetInfo>)value;
            IconOfJaw tempp = (IconOfJaw)Enum.Parse(typeof(IconOfJaw), (string)parameter, false);
            bool had = source.Any(x => x.BetName == tempp);
            if (had)
            {
                return source.Where(x => x.BetName == tempp).First().BetValue.ToString();
            }
            else
                return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
