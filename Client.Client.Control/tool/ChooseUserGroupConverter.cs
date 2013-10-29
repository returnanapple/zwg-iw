using Client.Client.Control.ChatService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Client.Client.Control
{
    public class ChooseUserGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserInfoType _type = (UserInfoType)value;
            return _type.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value != true) { return null; }
            return (UserInfoType)Enum.Parse(typeof(UserInfoType), parameter.ToString(), false);
        }
    }
}
