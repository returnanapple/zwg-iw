using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 关于表格工具展示类型的转换器（绑定于 Foreground）
    /// </summary>
    public class TableTypeConverter : IValueConverter
    {
        /// <summary>
        /// 在将源数据传递到目标以在 UI 中显示之前，对源数据进行修改
        /// </summary>
        /// <param name="value">正传递到目标的源数据</param>
        /// <param name="targetType">目标依赖项属性需要的数据的 System.Type</param>
        /// <param name="parameter">要在转换器逻辑中使用的可选参数</param>
        /// <param name="culture">转换的区域性</param>
        /// <returns>要传递到目标依赖项属性的值</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                return value.ToString() == parameter.ToString();
            }

            return false;
        }

        /// <summary>
        /// 在将目标数据传递到源对象之前，对目标数据进行修改。此方法仅在 System.Windows.Data.BindingMode.TwoWay 绑定中进行调用
        /// </summary>
        /// <param name="value">正传递到源的目标数据</param>
        /// <param name="targetType">源对象需要的数据的 System.Type</param>
        /// <param name="parameter">要在转换器逻辑中使用的可选参数</param>
        /// <param name="culture">转换的区域性</param>
        /// <returns>要传递到源对象的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return "";
            }
            if ((bool)value == false)
            {
                return "";
            }

            return parameter;
        }
    }
}
